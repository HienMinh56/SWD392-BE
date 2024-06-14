using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.PageModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IStoreSessionRepository _storeSession;
        private readonly ISessionRepository _session;
        private readonly IMapper _mapper;

        public StoreService(IStoreRepository storeRepository, IStoreSessionRepository storeSession, ISessionRepository session, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _storeSession = storeSession;
            _session = session;
            _mapper = mapper;
        }
        

        public async Task<string> GenerateStoreSessionIdAsync()
        {
            // Get the latest StoreSessionId from the database
            var latestStoreSession = await _storeSession.GetLatestStoreSession();

            // Extract the numeric part from the latest StoreSessionId
            int latestNumber = 0;
            if (latestStoreSession != null && int.TryParse(latestStoreSession.StoreSessionId.Replace("STORESESSION", ""), out latestNumber))
            {
                latestNumber++; // Increment the numeric part
            }
            else
            {
                latestNumber = 1; // Start from 1 if no previous StoreSessionId found
            }

            // Format the new StoreSessionId
            string newStoreSessionId = $"STORESESSION{latestNumber.ToString("D3")}";

            return newStoreSessionId;
        }

        public async Task<string> GenerateNewStoreIdAsync()
        {
            var lastStoreId = await _storeRepository.GetLastStoreIdAsync();
            int newIdNumber = 1;

            if (!string.IsNullOrEmpty(lastStoreId))
            {
                // Extract numeric part and increment it
                int.TryParse(lastStoreId.Substring(5), out newIdNumber);
                newIdNumber++;
            }

            // Format the new ID with leading zeros
            string newStoreId = $"STORE{newIdNumber:D3}";
            return newStoreId;
        }


        

        public Store GetStoreById(string id)
        {
            try
            {
                var data = _storeRepository.Get(x => x.StoreId == id);

                if (data == null)
                {
                    // Handle the case where the user is not found, e.g., return null or throw an exception
                    return null;
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResultModel> addStore(StoreViewModel model, ClaimsPrincipal userCreate)
        {
            ResultModel result = new ResultModel();
            try
            {
                string newStoreId = await GenerateNewStoreIdAsync();

                if (model == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Store request model is null.";
                    return result;
                }
                var extistingStore = _storeRepository.Get(s => s.AreaId == model.AreaId && s.Address == model.Address);
                if (extistingStore != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "At Address had store";
                    return result;
                }

                var existingPhone = _storeRepository.Get(s => s.Phone == model.Phone);

                if (existingPhone != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Phone has been used";
                    return result;
                }

                var matchingSessions = await _session.FindAsync(s =>
                (TimeSpan.Parse(model.OpenTime) >= s.StartTime &&
                TimeSpan.Parse(model.OpenTime) <= s.EndTime) ||
                (TimeSpan.Parse(model.CloseTime) >= s.StartTime &&
                TimeSpan.Parse(model.CloseTime) <= s.EndTime));

                var matchingSession = matchingSessions.FirstOrDefault(); // Get the first matching session

                if (matchingSession == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "No session found for the provided open and close times.";
                    return result;
                }

                var newStore = _mapper.Map<Store>(model);
                newStore.StoreId = newStoreId;
                newStore.CreatedBy = userCreate.FindFirst("UserName")?.Value;
                newStore.CreatedDate = DateTime.Now;
                newStore.Status = 1;
                newStore.OpenTime = TimeSpan.Parse(model.OpenTime);
                newStore.CloseTime = TimeSpan.Parse(model.CloseTime);

                _storeRepository.Add(newStore);
                _storeRepository.SaveChanges();

                var newStoreSessionId = await GenerateStoreSessionIdAsync();
                // Add the store and session ID to the StoreSession table
                var storeSession = new StoreSession
                {
                    StoreSessionId = newStoreSessionId,
                    StoreId = newStoreId,
                    SessionId = matchingSession.SessionId // Assuming SessionId is unique and matches the session found
                };
                _storeSession.Add(storeSession);
                _storeSession.SaveChanges();

                result.IsSuccess = true;
                result.Code = 200;
                result.Message = "Add Store Success";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = ex.Message;
                return result;
            }
        }


        public async Task<ResultModel> GetStoresByStatusAreaAndSessionAsync(int? status, string? areaName, string? sessionId, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var stores = await _storeRepository.FetchStoresAsync();

                if (status.HasValue)
                {
                    stores = stores.Where(s => s.Status == status.Value).ToList();
                }

                if (!string.IsNullOrEmpty(areaName))
                {
                    stores = stores.Where(s => s.Area.Name == areaName).ToList();
                }

                if (!string.IsNullOrEmpty(sessionId))
                {
                    stores = stores.Where(s => s.StoreSessions.Any(ss => ss.SessionId == sessionId)).ToList();
                }

                var totalItems = stores.Count;
                var pagedStores = stores.Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                if (!pagedStores.Any())
                {
                    result.IsSuccess = true;
                    result.Code = 201;
                    result.Message = "No stores found";
                }
                else
                {
                    var storeViewModels = pagedStores.Select(s => new GetStoreViewModel
                    {
                        StoreId = s.StoreId,
                        AreaId = s.AreaId,
                        Name = s.Name,
                        Address = s.Address,
                        Status = s.Status,
                        Phone = s.Phone,
                        OpenTime = s.OpenTime,
                        CloseTime = s.CloseTime,
                        AreaName = s.Area.Name,
                        Session = s.StoreSessions.Select(ss => ss.SessionId.ToString()).ToList()
                    }).ToList();

                    var pagedResult = new PagedResultViewModel<GetStoreViewModel>
                    {
                        TotalItems = totalItems,
                        PageNumber = pageIndex,
                        PageSize = pageSize,
                        Items = storeViewModels
                    };

                    result.IsSuccess = true;
                    result.Code = 200;
                    result.Message = "Stores retrieved successfully";
                    result.Data = pagedResult;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 500;
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }



        public async Task<ResultModel> UpdateStoreAsync(string storeId, UpdateStoreViewModel model, ClaimsPrincipal userUpdate)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (model == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Store request model is null.";
                    return result;
                }

                var existingStore = _storeRepository.Get(s => s.StoreId==storeId);
                if (existingStore == null)
                {
                    result.IsSuccess = false;
                    result.Code = 404;
                    result.Message = "Store not found.";
                    return result;
                }
                var phoneStore = _storeRepository.Get(s => s.Phone == model.Phone && s.StoreId != storeId);
                if(phoneStore != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Phone was used";
                    return result;
                }
                var addressStore = _storeRepository.Get(s => s.Address == model.Address && s.AreaId == model.AreaId && s.StoreId != storeId);
                if(addressStore != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Address had store";
                    return result ;
                }
               
                // Map the ViewModel to the existing store entity
                _mapper.Map(model, existingStore);

                // Update the additional fields
                existingStore.ModifiedBy = userUpdate.FindFirst("UserName")?.Value;
                existingStore.ModifiedDate = DateTime.UtcNow;
                existingStore.OpenTime = TimeSpan.Parse(model.OpenTime);
                existingStore.CloseTime = TimeSpan.Parse(model.CloseTime);

                _storeRepository.Update(existingStore);
                await _storeRepository.SaveChangesAsync();

                result.IsSuccess = true;
                result.Code = 200;
                result.Message = "Update Store Success";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 500;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<ResultModel> DeleteStore(DeleteStoreReqModel request, ClaimsPrincipal userDelete)
        {
            var result = new ResultModel();
            try
            {
                var store = GetStoreById(request.StoreId);
                if (store == null)
                {
                    result.Message = "Store not found or deleted";
                    result.Code = 404;
                    result.IsSuccess = false;
                    result.Data = null;
                    return result;
                }
                store.DeletedBy = userDelete.FindFirst("UserName")?.Value;
                store.DeletedDate = DateTime.UtcNow;
                store.Status = store.Status = 2;
                _storeRepository.Update(store);
                _storeRepository.SaveChanges();

                result.Message = "Delete Store successfully";
                result.Code = 200;
                result.IsSuccess = true;
                result.Data = store;
            }
            catch (DbUpdateException ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }
    }
}

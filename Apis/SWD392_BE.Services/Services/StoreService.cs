using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
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
        private readonly IMapper _mapper;

        public StoreService(IStoreRepository storeRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _mapper = mapper;
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
                var existingPhone = _storeRepository.Get(u => u.Phone.Equals(model.Phone));
                if (existingPhone != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Phone had been used";
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

        public async Task<ResultModel> getListStore()
        {
            ResultModel result = new ResultModel();
            try
            {
                var stores = _storeRepository.GetAll();
                if (stores == null || !stores.Any())
                {
                    result.IsSuccess = true;
                    result.Code = 201;
                    result.Message = "No store here";
                }
                else
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.Message = "Stores retrieved successfully";
                    result.Data = stores;
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
                var phoneStore = _storeRepository.Get(s => s.Phone == model.Phone);
                if(phoneStore != null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Phone was used";
                    return result;
                }
                var addressStore = _storeRepository.Get(s => s.Address == model.Address && s.AreaId == model.AreaId);
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

    }
}

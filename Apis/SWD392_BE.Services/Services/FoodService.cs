﻿using AutoMapper;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.FoodModel;
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
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public FoodService(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }
        public async Task<ResultModel> addFood(string storeId, List<List<FoodViewModel>> foodLists, ClaimsPrincipal userCreate)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (foodLists == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Food request model is null.";
                    return result;
                }

                string lastFoodId = await _foodRepository.GetLastFoodIdAsync();
                int foodIdCounter = lastFoodId != null ? int.Parse(lastFoodId.Substring(4)) + 1 : 1; // Bắt đầu từ FoodId cuối cùng + 1

                foreach (var foods in foodLists)
                {
                    foreach (var food in foods)
                    {
                        string newFoodId = "FOOD" + foodIdCounter.ToString("D3"); // Tạo FoodId theo thứ tự tăng dần
                        var newFood = _mapper.Map<Food>(food);
                        newFood.FoodId = newFoodId;
                        newFood.StoreId = storeId;
                        newFood.Name = food.Name;
                        newFood.Price = food.Price;
                        newFood.Title = food.Title;
                        newFood.Description = food.Description;
                        newFood.Cate = food.Cate;
                        newFood.Image = food.Image;
                        newFood.Status = 1;
                        newFood.CreatedBy = userCreate.FindFirst("UserName")?.Value;
                        newFood.CreatedDate = DateTime.Now;

                        _foodRepository.Add(newFood);

                        foodIdCounter++; // Tăng biến đếm sau mỗi lần thêm món ăn
                    }
                }

                _foodRepository.SaveChanges();

                result.IsSuccess = true;
                result.Code = 200;
                result.Message = "Add Food Success";
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



        public async Task<ResultModel> getListFood(string storeId)
        {
            ResultModel result = new ResultModel();
            try
            {
                var foods = _foodRepository.GetList(s => s.StoreId == storeId);
                if (foods == null)
                {
                    result.IsSuccess = true;
                    result.Code = 201;
                    result.Message = "No food here";
                }
                else
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.Message = "Foods retrieved successfully";
                    result.Data = foods;
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
        public async Task<ResultModel> UpdateFoodAsync(string id, UpdateFoodViewModel model, ClaimsPrincipal userUpdate)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (model == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Food request model is null.";
                    return result;
                }

                if (_foodRepository == null)
                {
                    result.IsSuccess = false;
                    result.Code = 500;
                    result.Message = "Food repository is not initialized.";
                    return result;
                }

                var existingFood = _foodRepository.Get(x => x.FoodId == id);
                if (existingFood == null)
                {
                    result.IsSuccess = false;
                    result.Code = 404;
                    result.Message = "Food not found.";
                    return result;
                }

                _mapper.Map(model, existingFood);
                existingFood.Price = model.Price;
                existingFood.Title = model.Title;
                existingFood.Description = model.Description;
                existingFood.Cate = model.Cate;
                existingFood.Image = model.Image;

                var modifiedBy = userUpdate.FindFirst("UserName")?.Value;
                if (string.IsNullOrEmpty(modifiedBy))
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "User name claim not found.";
                    return result;
                }

                existingFood.ModifiedBy = modifiedBy;
                existingFood.ModifiedDate = DateTime.Now;
                _foodRepository.Update(existingFood);
                await _foodRepository.SaveChangesAsync();

                result.IsSuccess = true;
                result.Code = 200;
                result.Message = "Update Food Success";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 500;
                result.Message = $"Exception: {ex.Message}";
                return result;
            }
        }
    }
}

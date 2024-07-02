﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.PageModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
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
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public FoodService(ICloudStorageService cloudStorageService, IFoodRepository foodRepository, IMapper mapper)
        {
            _cloudStorageService = cloudStorageService;
            _foodRepository = foodRepository;
            _mapper = mapper;
        }
        public Food GetFoodById(string id)
        {
            try
            {
                var data = _foodRepository.Get(x => x.FoodId == id);

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
        public async Task<ResultModel> addFood(string storeId, List<FoodViewModel> foodLists, ClaimsPrincipal userCreate)
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


                foreach (var food in foodLists)
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
                    newFood.Status = 1;
                    newFood.CreatedBy = userCreate.FindFirst("UserName")?.Value;
                    newFood.CreatedDate = DateTime.Now;

                    if (food.Image != null)
                    {
                        string imageFileName = $"{newFoodId}_{food.Image.FileName}";
                        newFood.Image = await _cloudStorageService.UploadFileAsync(food.Image, imageFileName);
                    }


                    _foodRepository.Add(newFood);

                    foodIdCounter++; // Tăng biến đếm sau mỗi lần thêm món ăn
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


        public async Task<ResultModel> GetListFoodsAsync(string storeId, int? cate)
        {
            ResultModel result = new ResultModel();
            try
            {
                // Retrieve store name
                string storeName = await _foodRepository.GetStoreNameAsync(storeId);

                // Retrieve and filter foods based on storeId and optional category
                var filteredFoods = _foodRepository.GetList(s => s.StoreId == storeId).ToList();

                if (cate.HasValue)
                {
                    filteredFoods = filteredFoods.Where(f => f.Cate == cate.Value).ToList();
                }

                // Calculate total number of foods
                int totalFoods = await _foodRepository.GetTotalFoodsAsync(storeId, cate);

                // Calculate total number of orders
                int totalOrders = await _foodRepository.GetTotalOrdersAsync(storeId);

                if (filteredFoods == null)
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
                    result.Data = new
                    {
                        StoreName = storeName,
                        TotalFoods = totalFoods,
                        TotalOrders = totalOrders,
                        Foods = filteredFoods
                    };
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



        public async Task<ResultModel> UpdateFoodAsync(string id, UpdateFoodViewModel model, ClaimsPrincipal userUpdate, IFormFile image)
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

                var existingFood = _foodRepository.Get(x => x.FoodId == id);
                if (existingFood == null)
                {
                    result.IsSuccess = false;
                    result.Code = 404;
                    result.Message = "Food not found.";
                    return result;
                }

                // Update food properties except image
                _mapper.Map(model, existingFood);
                existingFood.Price = model.Price;
                existingFood.Title = model.Title;
                existingFood.Description = model.Description;
                existingFood.Cate = model.Cate;

                // Handle image upload if imageFile is provided
                if (image != null && image.Length > 0)
                {
                    // Generate unique filename or use existing logic to determine filename
                    string fileNameToSave = $"food_{existingFood.FoodId}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

                    // Upload image to cloud storage
                    string imageUrl = await _cloudStorageService.UploadFileAsync(image, fileNameToSave);

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        // Update food's image URL
                        existingFood.Image = imageUrl;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Code = 500;
                        result.Message = "Failed to upload image to cloud storage.";
                        return result;
                    }
                }

                // Set modified by and date
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

                // Update and save changes
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
        public async Task<ResultModel> DeleteFood(DeleteFoodReqModel request, ClaimsPrincipal userDelete)
        {
            var result = new ResultModel();
            try
            {
                var food = GetFoodById(request.FoodId);
                if (food == null)
                {
                    result.Message = "Food not found or deleted";
                    result.Code = 404;
                    result.IsSuccess = false;
                    result.Data = null;
                    return result;
                }
                food.DeletedBy = userDelete.FindFirst("UserName")?.Value;
                food.DeletedDate = DateTime.UtcNow;
                food.Status = food.Status = 2;
                _foodRepository.Update(food);
                _foodRepository.SaveChanges();

                result.Message = "Delete Food successfully";
                result.Code = 200;
                result.IsSuccess = true;
                result.Data = food;
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

using AutoMapper;
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
        public async Task<string> GenerateNewFoodIdAsync()
        {
            var lastFoodId = await _foodRepository.GetLastFoodIdAsync();
            int newIdNumber = 1;

            if (!string.IsNullOrEmpty(lastFoodId))
            {
                // Extract numeric part and increment it
                int.TryParse(lastFoodId.Substring(5), out newIdNumber);
                newIdNumber++;
            }

            // Format the new ID with leading zeros
            string newFoodId = $"FOOD{newIdNumber:D3}";
            return newFoodId;
        }
        public async Task<ResultModel> addFood(string storeId, List<FoodViewModel> foods, ClaimsPrincipal userCreate)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (foods == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Food request model is null.";
                    return result;
                }

                foreach (var food in foods)
                {
                    string newFoodId = await GenerateNewFoodIdAsync();
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

        public async Task<ResultModel> getListFood()
        {
            ResultModel result = new ResultModel();
            try
            {
                var foods = _foodRepository.GetAll();
                if (foods == null || !foods.Any())
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
        public async Task<ResultModel> updateFood(string foodId, FoodViewModel model, ClaimsPrincipal userUpdate)
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
                var existingFood = _foodRepository.Get(u => u.FoodId.Equals(foodId));
                if (existingFood == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.Message = "Food Not Found";
                }

                var foodupdate = _mapper.Map<Food>(model);
                foodupdate.Price = model.Price;
                foodupdate.Title = model.Title;
                foodupdate.Description = model.Description;
                foodupdate.Cate = model.Cate;
                foodupdate.Image = model.Image;
                foodupdate.ModifiedBy = userUpdate.FindFirst("UserName")?.Value;
                foodupdate.ModifiedDate = DateTime.Now;
                _foodRepository.Update(foodupdate);
                _foodRepository.SaveChanges();
                result.IsSuccess = true;
                result.Code = 200;
                result.Message = "Update Food Success";
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
    }
}

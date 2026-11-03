using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Interfaces;

namespace FoodExpress.Infrastructure.Services
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _categoryRepository;

        public MenuCategoryService(
            IMenuCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<MenuCategoryResponseDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(MapToResponseDto).ToList();
        }

        public async Task<MenuCategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            return category == null ? null : MapToResponseDto(category);
        }

        public async Task<MenuCategoryResponseDto> CreateAsync(
            MenuCategoryCreateDto categoryDto)
        {
            var category = new MenuCategory
            {
                Name = categoryDto.Name,
                RestaurantId = categoryDto.RestaurantId
            };

            var createdCategory =
                await _categoryRepository.AddAsync(category);

            return MapToResponseDto(createdCategory);
        }

        public async Task UpdateAsync(
            int id,
            MenuCategoryCreateDto categoryDto)
        {
            var existingCategory =
                await _categoryRepository.GetByIdAsync(id);

            if (existingCategory == null)
                return;

            existingCategory.Name = categoryDto.Name;
            existingCategory.RestaurantId = categoryDto.RestaurantId;

            await _categoryRepository.UpdateAsync(existingCategory);
        }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        private static MenuCategoryResponseDto MapToResponseDto(
            MenuCategory category)
        {
            return new MenuCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                RestaurantId = category.RestaurantId
            };
        }
    }
}

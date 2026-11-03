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
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<IEnumerable<MenuItemResponseDto>> GetAllAsync()
        {
            var items = await _menuItemRepository.GetAllAsync();

            return items.Select(MapToResponseDto).ToList();
        }

        public async Task<MenuItemResponseDto?> GetByIdAsync(int id)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);

            return item == null ? null : MapToResponseDto(item);
        }

        public async Task<MenuItemResponseDto> CreateAsync(
            MenuItemCreateDto menuItemDto)
        {
            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price,
                MenuCategoryId = menuItemDto.MenuCategoryId,
                IsAvailable = true
            };

            var createdItem =
                await _menuItemRepository.AddAsync(menuItem);

            return MapToResponseDto(createdItem);
        }

        public async Task UpdateAsync(
            int id,
            MenuItemCreateDto menuItemDto)
        {
            var existingItem =
                await _menuItemRepository.GetByIdAsync(id);

            if (existingItem == null)
                return;

            existingItem.Name = menuItemDto.Name;
            existingItem.Description = menuItemDto.Description;
            existingItem.Price = menuItemDto.Price;
            existingItem.MenuCategoryId = menuItemDto.MenuCategoryId;

            await _menuItemRepository.UpdateAsync(existingItem);
        }

        public async Task DeleteAsync(int id)
        {
            await _menuItemRepository.DeleteAsync(id);
        }

        private static MenuItemResponseDto MapToResponseDto(
            MenuItem item)
        {
            return new MenuItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                MenuCategoryId = item.MenuCategoryId,
                IsAvailable = item.IsAvailable
            };
        }
    }
}

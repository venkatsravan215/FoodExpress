using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface IMenuCategoryService
    {
        Task<IEnumerable<MenuCategoryResponseDto>> GetAllAsync();

        Task<MenuCategoryResponseDto?> GetByIdAsync(int id);

        Task<MenuCategoryResponseDto> CreateAsync(
            MenuCategoryCreateDto categoryDto);

        Task UpdateAsync(
            int id,
            MenuCategoryCreateDto categoryDto);

        Task DeleteAsync(int id);
    }
}


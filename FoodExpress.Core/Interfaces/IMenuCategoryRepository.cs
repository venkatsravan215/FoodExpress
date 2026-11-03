using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface IMenuCategoryRepository
    {
        Task<IEnumerable<MenuCategory>> GetAllAsync();

        Task<MenuCategory?> GetByIdAsync(int id);

        Task<MenuCategory> AddAsync(MenuCategory category);

        Task UpdateAsync(MenuCategory category);

        Task DeleteAsync(int id);
    }
}

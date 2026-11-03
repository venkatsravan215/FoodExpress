using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;
using FoodExpress.Core.Interfaces;
using FoodExpress.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodExpress.Infrastructure.Repositories
{
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly FoodExpressDbContext _context;

        public MenuCategoryRepository(FoodExpressDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuCategory>> GetAllAsync()
        {
            return await _context.MenuCategories.ToListAsync();
        }

        public async Task<MenuCategory?> GetByIdAsync(int id)
        {
            return await _context.MenuCategories.FindAsync(id);
        }

        public async Task<MenuCategory> AddAsync(MenuCategory category)
        {
            await _context.MenuCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task UpdateAsync(MenuCategory category)
        {
            _context.MenuCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);

            if (category == null)
                return;

            _context.MenuCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}

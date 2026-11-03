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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly FoodExpressDbContext _context;

        public CartItemRepository(FoodExpressDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(ci => ci.MenuItem)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<CartItem?> GetByCartAndMenuItemAsync(
            int cartId,
            int menuItemId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cartId &&
                    ci.MenuItemId == menuItemId);
        }

        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
                return;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Interfaces;
using FoodExpress.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodExpress.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly FoodExpressDbContext _context;

        public CartRepository(FoodExpressDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task UpdateAsync(Cart cart)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
                return;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(
            int cartId,
            int menuItemId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(item =>
                    item.CartId == cartId &&
                    item.MenuItemId == menuItemId);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}

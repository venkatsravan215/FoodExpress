
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Interfaces;

namespace FoodExpress.Infrastructure.Services
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public CartService(
            ICartRepository cartRepository,
            IMenuItemRepository menuItemRepository)
        {
            _cartRepository = cartRepository;
            _menuItemRepository = menuItemRepository;
        }


        public async Task<IEnumerable<CartResponseDto>> GetAllAsync()
        {
            var carts = await _cartRepository.GetAllAsync();
            return carts.Select(MapToResponseDto).ToList();
        }

        public async Task<CartResponseDto?> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            return cart == null ? null : MapToResponseDto(cart);
        }

        public async Task<CartResponseDto?> GetByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            return cart == null ? null : MapToResponseDto(cart);
        }

        public async Task<CartResponseDto> CreateAsync(CartCreateDto cartDto)
        {
            var existingCart =
                await _cartRepository.GetByUserIdAsync(cartDto.UserId);

            if (existingCart != null)
                throw new InvalidOperationException(
                    "This user already has a cart.");

            var cart = new Cart
            {
                UserId = cartDto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdCart = await _cartRepository.AddAsync(cart);
            return MapToResponseDto(createdCart);
        }

        public async Task<CartResponseDto> AddItemAsync(
            int cartId, int userId, int menuItemId, int quantity)
        {
            if (quantity <= 0)
                throw new InvalidOperationException(
                    "Quantity must be greater than zero.");

            var cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart == null || cart.UserId != userId)
                throw new KeyNotFoundException("Cart not found.");

            var existingItem = await _cartRepository.GetCartItemAsync(
                cartId, menuItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);

                if (menuItem == null)
                    throw new KeyNotFoundException("Menu item not found.");

                if (menuItem == null)
                    throw new KeyNotFoundException(
                        "Menu item not found in the cart data. Verify that the menu item exists.");

                var cartItem = new CartItem
                {
                    CartId = cartId,
                    MenuItemId = menuItemId,
                    Quantity = quantity,
                    UnitPrice = menuItem.Price
                };

                await _cartRepository.AddCartItemAsync(cartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);

            var updatedCart = await _cartRepository.GetByIdAsync(cartId);
            return MapToResponseDto(updatedCart!);
        }

        public async Task<CartResponseDto> UpdateItemQuantityAsync(
            int cartId, int userId, int menuItemId, int quantity)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart == null || cart.UserId != userId)
                throw new KeyNotFoundException("Cart not found.");

            var item = await _cartRepository.GetCartItemAsync(
                cartId, menuItemId);

            if (item == null)
                throw new KeyNotFoundException("Cart item not found.");

            if (quantity <= 0)
                await _cartRepository.DeleteCartItemAsync(item);
            else
            {
                item.Quantity = quantity;
                await _cartRepository.UpdateCartItemAsync(item);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cart);

            var updatedCart = await _cartRepository.GetByIdAsync(cartId);
            return MapToResponseDto(updatedCart!);
        }

        public async Task DeleteAsync(int id)
        {
            await _cartRepository.DeleteAsync(id);
        }

        private static CartResponseDto MapToResponseDto(Cart cart)
        {
            var items = cart.CartItems.Select(item =>
                new CartItemResponseDto
                {
                    Id = item.Id,
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Quantity = item.Quantity,
                    Subtotal = item.MenuItem.Price * item.Quantity
                }).ToList();

            return new CartResponseDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = items,
                TotalAmount = items.Sum(item => item.Subtotal)
            };
        }
    }
}

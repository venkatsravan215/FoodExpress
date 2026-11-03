using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;

namespace FoodExpress.Infrastructure.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public CartItemService(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IMenuItemRepository menuItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<CartResponseDto> AddItemAsync(
            int cartId,
            CartItemCreateDto itemDto)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var menuItem =
                await _menuItemRepository.GetByIdAsync(itemDto.MenuItemId);

            if (menuItem == null)
                throw new KeyNotFoundException("Menu item not found.");

            if (!menuItem.IsAvailable)
                throw new InvalidOperationException(
                    "This menu item is currently unavailable.");

            var existingItem =
                await _cartItemRepository.GetByCartAndMenuItemAsync(
                    cartId, itemDto.MenuItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
                await _cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var cartItem = new FoodExpress.Core.Entities.CartItem
                {
                    CartId = cartId,
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity
                };

                await _cartItemRepository.AddAsync(cartItem);
            }

            var updatedCart = await _cartRepository.GetByIdAsync(cartId);

            return MapToResponseDto(updatedCart!);
        }

        public async Task UpdateQuantityAsync(
            int cartId,
            int cartItemId,
            int quantity)
        {
            if (quantity < 1 || quantity > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be between 1 and 100.");

            var cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var item = await _cartItemRepository.GetByIdAsync(cartItemId);

            if (item == null || item.CartId != cartId)
                throw new KeyNotFoundException(
                    "Cart item not found in this cart.");

            item.Quantity = quantity;

            await _cartItemRepository.UpdateAsync(item);
        }

        public async Task RemoveItemAsync(int cartId, int cartItemId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var item = await _cartItemRepository.GetByIdAsync(cartItemId);

            if (item == null || item.CartId != cartId)
                throw new KeyNotFoundException(
                    "Cart item not found in this cart.");

            await _cartItemRepository.DeleteAsync(cartItemId);
        }

        private static CartResponseDto MapToResponseDto(
            FoodExpress.Core.Entities.Cart cart)
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


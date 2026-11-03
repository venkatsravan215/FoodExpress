using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Enums;
using FoodExpress.Core.Interfaces;

namespace FoodExpress.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToResponseDto).ToList();
        }

        public async Task<OrderResponseDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            return order == null ? null : MapToResponseDto(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetByUserIdAsync(
            int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(MapToResponseDto).ToList();
        }

        public async Task<OrderResponseDto> CreateAsync(
            OrderCreateDto orderDto)
        {
            var cart = await _cartRepository.GetByUserIdAsync(
                orderDto.UserId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException(
                    "The user's cart is empty or does not exist.");
            }

            var subtotal = cart.CartItems.Sum(
                item => item.MenuItem.Price * item.Quantity);

            var order = new Order
            {
                UserId = orderDto.UserId,
                AddressId = orderDto.AddressId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                SubTotal = subtotal,
                DeliveryFee = 0,
                TotalAmount = subtotal
            };

            foreach (var cartItem in cart.CartItems)
            {
                var unitPrice = cartItem.MenuItem.Price;

                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = cartItem.MenuItemId,
                    ItemName = cartItem.MenuItem.Name,
                    Quantity = cartItem.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = unitPrice * cartItem.Quantity
                });
            }

            var createdOrder = await _orderRepository.AddAsync(order);

            return MapToResponseDto(createdOrder);
        }

        public async Task UpdateStatusAsync(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        private static OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                CreatedAt = order.CreatedAt,
                SubTotal = order.SubTotal,
                DeliveryFee = order.DeliveryFee,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = order.OrderItems.Select(item =>
                    new OrderItemResponseDto
                    {
                        Id = item.Id,
                        MenuItemId = item.MenuItemId,
                        ItemName = item.ItemName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
            };
        }
    }
}

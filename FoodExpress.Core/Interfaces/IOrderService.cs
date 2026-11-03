    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    using FoodExpress.Core.DTOs;

    namespace FoodExpress.Core.Interfaces
    {
        public interface IOrderService
        {
            Task<IEnumerable<OrderResponseDto>> GetAllAsync();

            Task<OrderResponseDto?> GetByIdAsync(int id);

            Task<IEnumerable<OrderResponseDto>> GetByUserIdAsync(int userId);

            Task<OrderResponseDto> CreateAsync(OrderCreateDto orderDto);

            Task UpdateStatusAsync(int id, FoodExpress.Core.Enums.OrderStatus status);

            Task DeleteAsync(int id);
        }
    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();

        Task<Restaurant?> GetByIdAsync(int id);

        Task<Restaurant> AddAsync(Restaurant restaurant);

        Task UpdateAsync(Restaurant restaurant);

        Task DeleteAsync(int id);
    }
}


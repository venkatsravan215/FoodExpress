using FoodExpress.Core.DTOs;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;     

namespace FoodExpress.Infrastructure.Services
    {
        public class RestaurantService : IRestaurantService
        {
            private readonly IRestaurantRepository _restaurantRepository;

            public RestaurantService(
                IRestaurantRepository restaurantRepository)
            {
                _restaurantRepository = restaurantRepository;
            }

            public async Task<IEnumerable<RestaurantResponseDto>> GetAllAsync()
            {
                var restaurants = await _restaurantRepository.GetAllAsync();

                return restaurants.Select(MapToResponseDto).ToList();
            }

            public async Task<RestaurantResponseDto?> GetByIdAsync(int id)
            {
                var restaurant = await _restaurantRepository.GetByIdAsync(id);

                return restaurant == null ? null : MapToResponseDto(restaurant);
            }

            public async Task<RestaurantResponseDto> CreateAsync(
                RestaurantCreateDto restaurantDto)
            {
                var restaurant = new Restaurant
                {
                    Name = restaurantDto.Name,
                    Description = restaurantDto.Description,
                    Address = restaurantDto.Address,
                    PhoneNumber = restaurantDto.PhoneNumber,
                    Email = restaurantDto.Email,
                    OwnerId = restaurantDto.OwnerId,
                    IsActive = true
                };

                var createdRestaurant =
                    await _restaurantRepository.AddAsync(restaurant);

                return MapToResponseDto(createdRestaurant);
            }


        public async Task UpdateAsync(
            int id,
            RestaurantUpdateDto restaurantDto)
        {
            var existingRestaurant =
                await _restaurantRepository.GetByIdAsync(id);

            if (existingRestaurant == null)
                return;

            existingRestaurant.Name = restaurantDto.Name;
            existingRestaurant.Description = restaurantDto.Description;
            existingRestaurant.Address = restaurantDto.Address;
            existingRestaurant.PhoneNumber = restaurantDto.PhoneNumber;
            existingRestaurant.Email = restaurantDto.Email;

            await _restaurantRepository.UpdateAsync(existingRestaurant);
        }


        public async Task DeleteAsync(int id)
            {
                await _restaurantRepository.DeleteAsync(id);
            }

            private static RestaurantResponseDto MapToResponseDto(
                Restaurant restaurant)
            {
                return new RestaurantResponseDto
                {
                    Id = restaurant.Id,
                    OwnerId = restaurant.OwnerId,
                    Name = restaurant.Name,
                    Description = restaurant.Description,
                    Address = restaurant.Address,
                    PhoneNumber = restaurant.PhoneNumber,
                    Email = restaurant.Email,
                    IsActive = restaurant.IsActive
                };
            }
        }
    }



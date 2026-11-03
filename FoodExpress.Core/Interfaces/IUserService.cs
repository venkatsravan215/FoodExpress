using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetByIdAsync(int id);

        Task<UserResponseDto?> GetByEmailAsync(string email);

        Task<IEnumerable<UserResponseDto>> GetAllAsync();

        Task<UserResponseDto> CreateAsync(UserCreateDto userDto);

        Task UpdateAsync(int id, UserUpdateDto userDto);

        Task DeleteAsync(int id);

        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);

    }
}
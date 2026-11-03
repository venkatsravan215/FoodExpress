using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}

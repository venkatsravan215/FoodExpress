using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FoodExpress.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RestaurantId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;

        public Restaurant Restaurant { get; set; } = null!;
    }
}

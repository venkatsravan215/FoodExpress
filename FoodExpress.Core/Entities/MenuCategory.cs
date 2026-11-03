using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodExpress.Core.Entities
{
    public class MenuCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int RestaurantId { get; set; }

        // Navigation Property
        public Restaurant Restaurant { get; set; } = null!;

        // One Category → Many Menu Items
        public ICollection<MenuItem> MenuItems { get; set; }
            = new List<MenuItem>();
    }
}

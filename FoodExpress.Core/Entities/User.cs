using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodExpress.Core.Enums;

namespace FoodExpress.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Address> Addresses { get; set; }
    = new List<Address>();

        public Cart? Cart { get; set; }

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();

        public ICollection<Order> Orders { get; set; }
    = new List<Order>();
    }
}

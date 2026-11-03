using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodExpress.Core.Enums;

namespace FoodExpress.Core.Entities
{
    public class Delivery
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int DeliveryPartnerId { get; set; }

        public DeliveryStatus Status { get; set; }

        public DateTime? AssignedAt { get; set; }

        public DateTime? PickedUpAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        // Navigation Properties
        public Order Order { get; set; } = null!;

        public User DeliveryPartner { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FoodExpress.Core.Enums
{
    public enum DeliveryStatus
    {
        WaitingForAssignment = 1,
        Assigned = 2,
        PickedUp = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Cancelled = 6
    }
}

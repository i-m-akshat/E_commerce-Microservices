

namespace OrderApi.Domain.Entities
{
public class Order
    {
        public int Id { get; set; } 
        public int ProductID { get; set; }
        public int ClientID { get;set;  }
        public int PurchaseQuantity { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.UtcNow;

    }
}

using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO dto)
        {
            return new Order
            {
                Id=dto.id,
                ClientID=dto.ClientId,
                OrderedDate=dto.OrderedDate,
                ProductID=dto.ProductId,    
                PurchaseQuantity=dto.PurchaseQuantity,
            };
        }
        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            // Case 1: A single order is provided
            if (order is not null && orders is null)
            {
                var singleOrder = new OrderDTO(
                    order.Id, order.ProductID,
                    order.ClientID, order.PurchaseQuantity,
                    order.OrderedDate
                );

                return (singleOrder, null);
            }

            // Case 2: A list of orders is provided
            if (orders is not null && order is null)
            {
                var _orders = orders.Select(x => new OrderDTO(
                    x.Id, x.ProductID,
                    x.ClientID, x.PurchaseQuantity,
                    x.OrderedDate
                )).ToList();

                return (null, _orders);
            }

            // If both are null or both are provided → invalid input
            return (null, null);
        }

    }
}

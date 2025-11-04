using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using Polly.Registry;
using System.Data;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _resiliencePipeline;
        private readonly IOrder _order;
        public OrderService(IOrder order,HttpClient httpClient,ResiliencePipelineProvider<string> resiliencePipeline)
        {
            _httpClient = httpClient; 
            _resiliencePipeline=resiliencePipeline;
            _order = order;
        }
        //Get product 
        public async Task<ProductDTO> GetProduct(int id)
        {
            //call product api using httpclient 
            //redirect this call to the api gateway since product api is not responding to outsiders.
            var getProduct = await _httpClient.GetAsync($"/api/products/{id}");
            if (!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
            
        }
        //Get User 
        public async Task<AppUserDto> GetUser(int userId)
        {
            //call user api using httpclient 
            //redirect this call to the api gateway since user api is not responding to outsiders.
            var getuser = await _httpClient.GetAsync($"/api/users/{userId}");
            if (!getuser.IsSuccessStatusCode)
            {
                return null!;
            }
            var user = await getuser.Content.ReadFromJsonAsync<AppUserDto>();
            return user!;
        }
        //Get order dto by id 
        public async Task<OrderDetailsDTO> GetOrderDetailsByOrderId(int orderId)
        {
            //prepare the order
            var order = await _order.FindByIdAsync(orderId);
            if(order is null|| order!.Id <= 0)
            {
                return null!;
            }
            //Get retry pipeline
            var retryPipeline = _resiliencePipeline.GetPipeline("my-retry-pipeline");

            //prepare product
            var productDTO =await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductID));


            //prepare client 
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientID));

            //populate order details
            return new OrderDetailsDTO(order.Id,
                productDTO.id,
                order.ClientID,
                appUserDTO.email,
                appUserDTO.telephoneNumber,
                appUserDTO.addresss,
                productDTO.ProductName,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Price*order.PurchaseQuantity,
                order.OrderedDate
                );
        }

        public Task<IEnumerable<OrderDTO>> GetOrdersByClientID(int clientID)
        {
            throw new NotImplementedException();
        }
    }
}

using Azure;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrder _order,IOrderService _orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders=await _order.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound("No Order detected in the database !");
            }

            var (_,list)=OrderConversion.FromEntity(null, orders);
            if (!list!.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(list);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _order.FindByIdAsync(id);
            if(order is null)
            {
                return NotFound("Order is not available");
            }
            var (_orderDTO, _) = OrderConversion.FromEntity(order, null);
            return Ok(_orderDTO);
        }
        [HttpGet("client/{clientid:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0)
            {
                return BadRequest("invalid data provided ");

            }
            var orders = await _order.GetOrdersAsync(o => o.ClientID == clientId);
            return !orders.Any() ? NotFound(null):Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderDetailsByid(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("Invalid id");
            }
            var orderDetails = await _orderService.GetOrderDetailsByOrderId(orderId);
            return orderDetails != null ? Ok(orderDetails) : NotFound("No records found !");
        } 
        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("InCompleted data submitted");
            }
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var Response = await _order.CreateAsync(getEntity);
            if (Response.Flag)
            {
                return Ok(Response);
            }
            else
            {
                return BadRequest(Response);
            }

        }
        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Incomplete data submitted");
            //}
            var getEntity=OrderConversion.ToEntity(orderDTO);
            var response = await _order.UpdateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
       [HttpDelete]
       public async Task<ActionResult<Response>> DeleteOrder(OrderDTO order)
        {
            //get id 
            int id = order.id;
            var res = await _order.DeleteAsync(id);
            return res.Flag?Ok(res):BadRequest(res);

        }
    }
}

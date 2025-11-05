using Ecommerce.SharedLibrary.Logs;
using Ecommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepo : IOrder
    {
        private readonly OrderDbContext _dbContext;
        public OrderRepo(OrderDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = _dbContext.Orders.Add(entity).Entity;
                await _dbContext.SaveChangesAsync();
                if (order.Id > 0)
                {
                    return new Response(true, "Order placed successfully!");
                }
                else
                {
                    return new Response(false, "Error occurred while placing order");
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error Occurred while placing order !");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var order = await FindByIdAsync(id);
                if ((order is null))
                {
                    return new Response(false, "Order not found !");
                }
                 _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return new Response(true, "Order Deleted");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error Occurred while deleting order !");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order
                    = await _dbContext.Orders.FindAsync(id);
                return order is not null ? order : null! ;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null!;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var _orders = await _dbContext.Orders.AsNoTracking().ToListAsync();
                return _orders is not null?_orders:null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null!;
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var _orders = await _dbContext.Orders.Where(predicate).FirstOrDefaultAsync();
                return _orders is not null ? _orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null!;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var _orders = await _dbContext.Orders.Where(predicate).ToListAsync();
                return _orders is not null ? _orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null!;
                throw;
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if(order is null)
                {
                    return new Response(false, "Order Not Found !");
                }
                //_dbContext.Entry(order).State = EntityState.Detached;
                //_dbContext.Orders.Update(entity);
                _dbContext.Entry(order).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return new Response(true, "Order updated successfully");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

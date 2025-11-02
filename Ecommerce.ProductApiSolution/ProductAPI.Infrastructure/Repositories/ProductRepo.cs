

using Ecommerce.SharedLibrary.Logs;
using Ecommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.Repositories
{
    //public class ProductRepo(ProductDbContext _context) : IProduct you can write it like this or 
    public class ProductRepo : IProduct
    {
        private readonly ProductDbContext _context;
        public ProductRepo(ProductDbContext context)
        {
            _context = context;
        }//or you can write it like this this is much convenient and easy to understand 
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                //check if already exists 
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, $"{entity.Name} already exists");
                }else
                {
                    var currentEntity =  _context.Products.Add(entity).Entity;
                    await _context.SaveChangesAsync();
                    if(currentEntity is not null&&currentEntity.Id>0)
                    {
                        return new Response(true, $"{entity.Name} added successfully");
                    }
                    else
                    {
                        return new Response(false, $"Error Occurred while adding new {entity.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                //logging the original exception
                LogException.LogExceptions(ex);
                //display scary free messages to client 
                return new Response(false, "Error Occurred adding new product");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (product != null) 
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return new Response(true, $"{product.Name} has been deleted successfully");
                }
                else
                {
                    return new Response(false, "Entity not found");
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Errror occurred while deleting");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product=await _context.Products.Where(x=>x.Id== id).FirstOrDefaultAsync();  
                if(product is not null)
                {
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var product = await _context.Products.AsNoTracking().ToListAsync();
                if(product is not null)
                {
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null;
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await _context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product is not null ? product : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null;
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (product is null)
                {
                    return new Response(false, $"{entity.Name} not found");
                }

                // Apply new values from the updated entity
                _context.Entry(product).CurrentValues.SetValues(entity);

                await _context.SaveChangesAsync();

                return new Response(true, $"{entity.Name} has been updated successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Some error occurred!");
            }

        }
    }
}

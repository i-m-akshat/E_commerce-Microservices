

using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Infrastructure.Data
{
    //public class ProductDbContext(DbContextOptions<ProductDbContext> options):DbContext(options)
    //  {
    //      public DbSet<Product> Products { get; set; }
    //  }
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }  
        
    }
}

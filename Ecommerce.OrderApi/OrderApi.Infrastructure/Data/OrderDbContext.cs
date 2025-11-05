using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entities;

namespace OrderApi.Infrastructure.Data
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }

    }
}
//add-migration InitialCreate -OutputDir Data/Migrations

 //use the above line for migrations etc 
using Microsoft.EntityFrameworkCore;
using Order.api.Models;

namespace Order.api.DataAcess
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
        {
           
        }

        public DbSet<Models.Order> Oder { get; set; }
        public DbSet<OrderDetail> OderDetail { get; set; }
    }
}

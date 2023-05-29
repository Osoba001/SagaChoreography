using Inventory.mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.mvc.DataAccess
{
    public class InventoryDbContext:DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options):base(options) { }

        public DbSet<InventoryModel> Inventory { get; set; }
    }
}

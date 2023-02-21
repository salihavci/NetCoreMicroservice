using FreeCourse.Services.Order.Infrastracture.ModelBuilders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastracture
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
        {
        }
        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OrderModelBuilder.InitModelCreating(modelBuilder, DEFAULT_SCHEMA);
            OrderItemModelBuilder.InitModelCreating(modelBuilder, DEFAULT_SCHEMA);
            base.OnModelCreating(modelBuilder);
        }

    }
}

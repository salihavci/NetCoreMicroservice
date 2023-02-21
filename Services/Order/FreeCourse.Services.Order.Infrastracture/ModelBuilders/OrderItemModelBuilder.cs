using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastracture.ModelBuilders
{
    public class OrderItemModelBuilder
    {
        public static void InitModelCreating(ModelBuilder modelBuilder, string schemeName)
        {
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", schemeName);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

        }
    }
}

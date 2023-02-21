using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastracture.ModelBuilders
{
    public static class OrderModelBuilder
    {
        public static void InitModelCreating(ModelBuilder modelBuilder, string schemeName)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", schemeName);
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(o => o.Address).WithOwner();
        }
    }
}

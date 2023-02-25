using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order.Domain.Entities.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
        {
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.StatusDescription).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.OrderItems)
                .WithOne(o => o.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

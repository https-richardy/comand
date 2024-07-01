namespace Comanda.WebApi.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(order => order.Establishment)
            .WithMany(establishment => establishment.Orders)
            .HasForeignKey(order => order.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(order => order.Customer)
            .WithMany(customer => customer.Orders)
            .HasForeignKey(order => order.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(order => order.ShippingAddress)
            .WithMany()
            .HasForeignKey(order => order.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
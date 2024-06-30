namespace Comanda.WebApi.Data.Repositories;

public sealed class OrderRepository(ComandaDbContext dbContext) :
    MinimalRepository<Order, ComandaDbContext>(dbContext),
    IOrderRepository
{

}
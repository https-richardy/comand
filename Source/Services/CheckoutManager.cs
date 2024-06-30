namespace Comanda.WebApi.Services;

public sealed class CheckoutManager(
    IEstablishmentRepository establishmentRepository,
    ICustomerRepository customerRepository,
    ICartRepository cartRepository,
    IOrderRepository orderRepository
)
{
    public async Task CheckoutAsync(int customerId, CheckoutRequest checkoutInfo)
    {
        var cart = await GetCustomerCartAsync(customerId);
        var order = await CreateOrderAsync(cart, checkoutInfo);

        await orderRepository.SaveAsync(order);
        await cartRepository.ClearCartAsync(cart);
    }

    private async Task<Cart> GetCustomerCartAsync(int customerId)
    {
        var customer = await customerRepository.RetrieveByIdAsync(customerId);
        if (customer is null)
            throw new CustomerNotFoundException(customerId);

        var cart = await cartRepository.FindCartWithItemsAsync(customerId);
        if (cart is null)
            throw new CartNotFoundException(customerId);

        return cart;
    }

    private ICollection<OrderItem> MapCartItemsToOrderItems(Cart cart)
    {
        var orderItems = new List<OrderItem>();

        foreach (CartItem item in cart.Items)
        {
            var product = item.Product;
            var orderItem = new OrderItem(item.Quantity, product);

            orderItems.Add(orderItem);
        }

        return orderItems;
    }

    private async Task<Order> CreateOrderAsync(Cart cart, CheckoutRequest checkoutInfo)
    {
        var order = new Order
        {
            Establishment = await DetermineEstablishmentAsync(cart),
            Customer = cart.Customer,
            ShippingAddress = await DetermineShippingAddressAsync(cart.Customer.Id, checkoutInfo.AddressId),
            PaymentMethod = checkoutInfo.PaymentMethod,
            Date = DateTime.Now,
            Items = MapCartItemsToOrderItems(cart),
        };

        return order;
    }

    private async Task<Establishment> DetermineEstablishmentAsync(Cart cart)
    {
        if (cart is null || cart.Items.Count == 0)
            throw new Exception("Cart is empty."); // TODO: Throw custom exception

        var item = cart.Items.First();

        // por enquanto assumir que o pedido deve ir para o mesmo estabelecimento que o estabelecimento do primeiro item.
        return await establishmentRepository.RetrieveByIdAsync(item.Product.Establishment.Id);
    }

    private async Task<Address> DetermineShippingAddressAsync(int customerId, int addressId)
    {
        var customer = await customerRepository.RetrieveByIdAsync(customerId);
        if (customer is null)
            throw new CustomerNotFoundException(customerId);

        return customer.Addresses
            .Where(address => address.Id == addressId)
            .First();
    }
}
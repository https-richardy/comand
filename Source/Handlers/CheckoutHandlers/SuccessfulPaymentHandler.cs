namespace Comanda.WebApi.Handlers;

public sealed class SuccessfulPaymentHandler(
    ICartRepository cartRepository,
    IOrderRepository orderRepository,
    ICheckoutManager checkoutManager
) : IRequestHandler<SuccessfulPaymentRequest, Response>
{
    public async Task<Response> Handle(
        SuccessfulPaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        var session = await checkoutManager.GetSessionAsync(request.SessionId);
        var cartId = int.Parse(session.Metadata["cartId"]);

        var cart = await cartRepository.RetrieveByIdAsync(cartId);

        /* manual mapping of a CartItem to an OrderItem. I have a deadline, but I swear I'll make it more readable.*/
        var items = cart.Items.Select(cartItem => new OrderItem
        {
            Quantity = cartItem.Quantity,
            Product = cartItem.Product,
            Additionals = cartItem.Additionals.Select(additional => new OrderItemAdditional
            {
                Additional = additional.Additional,
                Quantity = additional.Quantity
            }).ToList(),
            UnselectedIngredients = cartItem.UnselectedIngredients.Select(ingredient => new UnselectedIngredient
            {
                Ingredient = ingredient.Ingredient,
                CartItem = cartItem
            }).ToList()
        }).ToList();

        var order = new Order
        {
            Items = items,
            Customer = cart.Customer,
            Status = EOrderStatus.Pending,
            Date = DateTime.Now
        };

        await orderRepository.SaveAsync(order);

        return new Response(
            statusCode: StatusCodes.Status200OK,
            message: "payment and order created successfully."
        );
    }
}
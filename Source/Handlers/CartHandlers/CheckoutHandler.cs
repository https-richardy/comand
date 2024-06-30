namespace Comanda.WebApi.Handlers;

public sealed class CheckoutHandler(
    UserManager<Account> userManager,
    IValidator<CheckoutRequest> validator,
    ICustomerRepository customerRepository,
    ICartRepository cartRepository,
    CheckoutManager checkoutManager
) : IRequestHandler<CheckoutRequest, Response<CheckoutResponse>>
{
    public async Task<Response<CheckoutResponse>> Handle(
        CheckoutRequest request,
        CancellationToken cancellationToken
    )
    {
        var account = await userManager.FindByIdAsync(request.UserId);
        if (account is null)
            return new Response<CheckoutResponse>(
                data: null,
                statusCode: StatusCodes.Status404NotFound,
                message: "user not found."
            );

        var customer = await customerRepository.FindSingleAsync(customer => customer.Account.Id == account.Id);
        if (customer is null)
            return new Response<CheckoutResponse>(
                data: null,
                statusCode: StatusCodes.Status404NotFound,
                message: "customer not found."
            );

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await cartRepository.FindCartWithItemsAsync(customer.Id);
        if (cart is null)
            return new Response<CheckoutResponse>(
                data: null,
                statusCode: StatusCodes.Status404NotFound,
                message: "cart not found."
            );

        await checkoutManager.CheckoutAsync(customer.Id, request);
        return new Response<CheckoutResponse>(
            data: null,
            statusCode: StatusCodes.Status200OK,
            message: "order created successfully."
        );
    }
}
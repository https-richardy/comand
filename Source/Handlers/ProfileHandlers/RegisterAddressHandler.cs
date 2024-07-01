namespace Comanda.WebApi.Handlers;

public sealed class RegisterAddressHandler(
    UserManager<Account> userManager,
    IAddressService addressService,
    ICustomerRepository customerRepository,
    IValidator<RegisterAddressRequest> validator
) : IRequestHandler<RegisterAddressRequest, Response>
{
    public async Task<Response> Handle(
        RegisterAddressRequest request,
        CancellationToken cancellationToken
    )
    {
        var account = await userManager.FindByIdAsync(request.UserId);
        if (account is null)
            return new Response(statusCode: StatusCodes.Status404NotFound, message: "user not found");

        var customer = await customerRepository.FindSingleAsync(customer => customer.Account.Id == account.Id);
        if (customer is null)
            return new Response(statusCode: StatusCodes.Status404NotFound, message: "customer not found");

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var address = await addressService.GetByZipCodeAsync(request.PostalCode);

        address.Number = request.Number;
        customer.Addresses.Add(address);

        await customerRepository.UpdateAsync(customer);
        return new Response(statusCode: StatusCodes.Status200OK, "address registered successfully.");
    }
}

namespace Comanda.WebApi.Handlers;

public sealed class NewAddressRegistrationHandler(
    IUserContextService userContextService,
    IAddressRepository addressRepository,
    IAddressService addressService,
    ICustomerRepository customerRepository,
    IValidator<NewAddressRegistrationRequest> validator
) : IRequestHandler<NewAddressRegistrationRequest, Response>
{
    public async Task<Response> Handle(
        NewAddressRegistrationRequest request,
        CancellationToken cancellationToken
    )
    {
        var userIdentifier = userContextService.GetCurrentUserIdentifier();

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ValidationFailureResponse(errors: validationResult.Errors);

        #pragma warning disable CS8604
        var address = await addressService.GetByZipCodeAsync(request.PostalCode);
        var customer = await customerRepository.FindCustomerByUserIdAsync(userIdentifier);

        customer?.Addresses.Add(address);
        await addressRepository.SaveAsync(address);

        return new Response(
            statusCode: StatusCodes.Status201Created,
            message: "address successfully registered."
        );
    }
}
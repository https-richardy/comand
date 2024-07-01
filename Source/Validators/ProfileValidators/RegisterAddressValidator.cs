namespace Comanda.WebApi.Validators;

public class RegisterAddressValidator :
    AbstractValidator<RegisterAddressRequest>,
    IValidator<RegisterAddressRequest>
{
    public RegisterAddressValidator()
    {
        RuleFor(request => request.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .MaximumLength(8)
            .WithMessage("Postal code must not exceed 8 characters.");
    }
}
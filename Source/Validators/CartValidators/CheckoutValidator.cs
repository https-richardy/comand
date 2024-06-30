namespace Comanda.WebApi.Validators;

public sealed class CheckoutValidator : AbstractValidator<CheckoutRequest>, IValidator<CheckoutRequest>
{
    public CheckoutValidator()
    {
        RuleFor(request => request.AddressId)
            .NotEmpty().WithMessage("AddressId is required.");

        RuleFor(request => request.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method. Must be 'Pix' (0), 'Cash' (1), 'CreditCard' (2) or 'DebitCart' (3).");
    }
}
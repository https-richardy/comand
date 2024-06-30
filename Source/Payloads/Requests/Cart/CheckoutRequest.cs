namespace Comanda.WebApi.Payloads;

public sealed record CheckoutRequest : AuthenticatedRequest, IRequest<Response<CheckoutResponse>>
{
    public int AddressId { get; init; }
    public EPaymentMethod PaymentMethod { get; init; }
}
namespace Comanda.WebApi.Payloads;

public sealed record RegisterAddressRequest : AuthenticatedRequest, IRequest<Response>
{
    public string PostalCode { get; init; }
    public string Number { get; init; }
}
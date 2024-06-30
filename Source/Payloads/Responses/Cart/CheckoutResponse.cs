namespace Comanda.WebApi.Payloads;

public sealed class CheckoutResponse : Response
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string PixQrCode { get; set; }
    public decimal PaymentAmount { get; init; }
}
namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string CardNumber { get; } = default!;
    public string CardName { get; } = default!;
    public DateTime Expiration { get;  }
    public string CVV { get; } = default!;
    public int PaymentMethod { get; } = default!;
}
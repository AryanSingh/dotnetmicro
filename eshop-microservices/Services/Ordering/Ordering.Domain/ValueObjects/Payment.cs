namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string CardNumber { get; } = default!;
    public string CardName { get; } = default!;
    public DateTime Expiration { get;  }
    public string CVV { get; } = default!;
    public int PaymentMethod { get; } = default!;
    
    protected Payment(){}
    
    private Payment(string cardNumber, string cardName, DateTime expiration, string cvv, int paymentMethod)
    {
        CardNumber = cardNumber;
        CardName = cardName;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }
    
    public static Payment Of(string cardNumber, string cardName, DateTime expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
        if (expiration < DateTime.UtcNow)
        {
            throw new DomainException("Expiration date cannot be in the past.");
        }
        
        return new Payment(cardNumber, cardName, expiration, cvv, paymentMethod);
    }
}

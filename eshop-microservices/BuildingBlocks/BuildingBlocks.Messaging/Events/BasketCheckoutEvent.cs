namespace BuildingBlocks.Messaging.Events;

public record BasketCheckoutEvent: IntegrationEvent
{
    public Guid CustomerId { get; set; } = default!;
    public string UserName { get; init; } = default!;
    public decimal TotalPrice { get; set; } = default !;
    
    // shipping and billing address
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string EmailAddress { get; init; } = default!;
        public string AddressLine { get; init; } = default!;
        public string Country { get; init; } = default!;
        public string State { get; init; } = default!;
        public string ZipCode { get; init; } = default!;
    
        // payment
        public string CardNumber { get; init; } = default!;
        public string CardName { get; init; } = default!;
        public DateTime Expiration { get; init; } = default!;
        public int PaymentMethod { get; set; } = default!;
        public string CVV { get; init; } = default!;
    public List<BasketCheckoutOrderItem> OrderItems { get; set; } = new();

}
namespace Ordering.Application.Dtos;

public record PaymentDto(Guid Id, Guid OrderId, decimal Amount, string CardName, string CardNumber,  DateTime ExpirationDate, string Cvv, int PaymentMethod);
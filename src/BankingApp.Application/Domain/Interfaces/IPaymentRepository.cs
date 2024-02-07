using BankingApp.Application.Domain.Entities;

namespace BankingApp.Application.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetUserPayments(Guid userId);
    Task<bool> AddAsync(Payment payment);
}

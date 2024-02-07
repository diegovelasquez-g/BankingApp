using BankingApp.Application.Domain.Entities;

namespace BankingApp.Application.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GeyByEmailAsync(string email);
    Task<User?> AuthUserAsync(string email, string password);
    Task<bool> AddAsync(User user);
}

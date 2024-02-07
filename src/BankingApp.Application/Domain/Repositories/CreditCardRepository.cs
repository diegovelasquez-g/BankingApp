using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Shared;
using Dapper;

namespace BankingApp.Application.Domain.Repositories;

public class CreditCardRepository : ICreditCardRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public CreditCardRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<CreditCard>> GetCreditCardsByUserIdAsync(Guid userId)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "SELECT * FROM CreditCards WHERE UserId = @UserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);

            var creditCards = await connection.QueryAsync<CreditCard>(query, parameters);
            return creditCards;
        }
    }
}
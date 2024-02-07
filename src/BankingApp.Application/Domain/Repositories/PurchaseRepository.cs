using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Shared;
using Dapper;
using System.Data;

namespace BankingApp.Application.Domain.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public PurchaseRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> AddAsync(Purchase purchase)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "spCreatePurchase";
            var parameters = new DynamicParameters();
            parameters.Add("PurchaseId", purchase.PurchaseId);
            parameters.Add("CreditCardId", purchase.CreditCardId);
            parameters.Add("Description", purchase.Description);
            parameters.Add("Amount", purchase.Amount);
            parameters.Add("Date", purchase.Date);
            var recordsAffected = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            return recordsAffected > 0;
        }
    }

    public async Task<IEnumerable<Purchase>> GetCreditCardPurchasesAsync(Guid creditCardId)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "spGetPurchasesByCreditCard";
            var parameters = new DynamicParameters();
            parameters.Add("CreditCardId", creditCardId);
            return await connection.QueryAsync<Purchase>(query, parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public async Task<IEnumerable<Purchase>> GetUserPurchasesAsync(Guid userId)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "spGetPurchasesByUserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            return await connection.QueryAsync<Purchase>(query, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}

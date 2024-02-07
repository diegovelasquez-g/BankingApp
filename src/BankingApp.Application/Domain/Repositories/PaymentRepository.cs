using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Shared;
using Dapper;
using System.Data;

namespace BankingApp.Application.Domain.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public PaymentRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> AddAsync(Payment payment)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "spCreatePayment";
            var parameters = new DynamicParameters();
            parameters.Add("PaymentId", payment.PaymentId);
            parameters.Add("CreditCardId", payment.CreditCardId);
            parameters.Add("Date", payment.Date);
            parameters.Add("Amount", payment.Amount);
            var recordsAffected = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            return recordsAffected > 0;
        }
    }

    public async Task<IEnumerable<Payment>> GetUserPayments(Guid userId)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "spGetPaymentsByUserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            return await connection.QueryAsync<Payment>(query, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}

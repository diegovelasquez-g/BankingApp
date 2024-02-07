using BankingApp.Application.Shared;
using BankingApp.Shared.Dtos.Responses;
using Dapper;
using FluentValidation;
using MediatR;
using System.Data;

namespace BankingApp.Application.Features.Transactions.Queries;

public class GetUserTransactionsQuery : IRequest<IEnumerable<TransactionsResponse>>
{
    public Guid UserId { get; set; }
}

public class GetUserTransactionsQueryHandler : IRequestHandler<GetUserTransactionsQuery, IEnumerable<TransactionsResponse>>
{
    private readonly IConnectionFactory _connectionFactory;

    public GetUserTransactionsQueryHandler(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<TransactionsResponse>> Handle(GetUserTransactionsQuery request, CancellationToken cancellationToken)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        using (connection)
        {
            var query = "spGetCreditCardTransactionsByUserId";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", request.UserId);
            return await connection.QueryAsync<TransactionsResponse>(query, parameters, commandType: CommandType.StoredProcedure);
        }
    }

    public class CreatePurchaseValidator : AbstractValidator<GetUserTransactionsQuery>
    {
        public CreatePurchaseValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
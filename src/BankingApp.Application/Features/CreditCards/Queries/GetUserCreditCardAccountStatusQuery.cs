using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Shared;
using BankingApp.Shared.Dtos.Responses;
using Dapper;
using FluentValidation;
using MediatR;
using System.Data;

namespace BankingApp.Application.Features.CreditCards.Queries;

public class GetUserCreditCardAccountStatusQuery : IRequest<CreditCardAccountStatusResponse>
{
    public Guid CreditCardId { get; set; }
}

public class GetUserCreditCardAccountStatusHandler : IRequestHandler<GetUserCreditCardAccountStatusQuery, CreditCardAccountStatusResponse>
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IPurchaseRepository _purchaseRepository;

    public GetUserCreditCardAccountStatusHandler(IConnectionFactory connectionFactory, IPurchaseRepository purchaseRepository)
    {
        _connectionFactory = connectionFactory;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<CreditCardAccountStatusResponse> Handle(GetUserCreditCardAccountStatusQuery request, CancellationToken cancellationToken)
    {
        var connection = _connectionFactory.Connection;
        if (connection is null)
            throw new Exception("Connection is null");

        var purchases = await _purchaseRepository.GetCreditCardPurchasesAsync(request.CreditCardId);

        using (connection)
        {
            var query = "spAccountStatus";
            var parameters = new DynamicParameters();
            parameters.Add("CreditCardId", request.CreditCardId);
            var result = await connection.QueryFirstAsync<CreditCardAccountStatusResponse>(query, parameters, commandType: CommandType.StoredProcedure);
            result.Purchases = purchases.Select(purchase => new PurchasesResponse
            {
                Date = purchase.Date,
                Description = purchase.Description,
                Amount = purchase.Amount,
            }).ToList();
            return result;
        }
    }

    public class GetUserCreditCardAccountStatusValidator : AbstractValidator<GetUserCreditCardAccountStatusQuery>
    {
        public GetUserCreditCardAccountStatusValidator()
        {
            RuleFor(x => x.CreditCardId).NotEmpty();
        }
    }
}

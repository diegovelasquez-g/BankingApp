using BankingApp.Application.Domain.Interfaces;
using BankingApp.Shared.Dtos.Responses;
using FluentValidation;
using MediatR;

namespace BankingApp.Application.Features.Payments.Queries;

public class GetUserPaymentsQuery : IRequest<IEnumerable<PaymentsResponse>>
{
    public Guid UserId { get; set; }
}

public class GetUserPaymentsQueryHandler : IRequestHandler<GetUserPaymentsQuery, IEnumerable<PaymentsResponse>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetUserPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<PaymentsResponse>> Handle(GetUserPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetUserPayments(request.UserId);
        return payments.Select(p => new PaymentsResponse
        {
            CreditCardId = p.CreditCardId,
            Amount = p.Amount,
            PaymentDate = p.Date
        });
    }

    public class CreatePurchaseValidator : AbstractValidator<GetUserPaymentsQuery>
    {
        public CreatePurchaseValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
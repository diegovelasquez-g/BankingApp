using AutoMapper;
using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Shared.Dtos.Responses;
using FluentValidation;
using MediatR;

namespace BankingApp.Application.Features.Users.Queries;

public class AuthUserQuery : IRequest<LoginResponse>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class AuthQueryHandler : IRequestHandler<AuthUserQuery, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(AuthUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GeyByEmailAsync(request.Email);
        if (user == null)
           throw new ValidationException("Invalid email or password");

        var _user = await _unitOfWork.Users.AuthUserAsync(request.Email, request.Password);
        if (_user == null)
            throw new ValidationException("Invalid email or password");

        return _mapper.Map<LoginResponse>(_user);
    }

    public class AuthUserQueryValidator : AbstractValidator<AuthUserQuery>
    {
        public AuthUserQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class AuthUserQueryProfile : Profile
    {
        public AuthUserQueryProfile() =>
            CreateMap<User, LoginResponse>();
    }
}
using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using FluentValidation;
using MediatR;
using AutoMapper;

namespace BankingApp.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<Unit>
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    //private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    //public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    //{
    //    _unitOfWork = unitOfWork;
    //}

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);

        //await _unitOfWork.Users.AddAsync(user);
        await _userRepository.AddAsync(user);
        return Unit.Value;
    }

    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }

    public class CreateUserCommandProfile : Profile
    {
        public CreateUserCommandProfile() =>
            CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid()));
    }

}

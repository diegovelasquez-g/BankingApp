using BankingApp.Application.Common.Behaviours;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Domain.Repositories;
using BankingApp.Application.Infraestructure.Persistance;
using BankingApp.Application.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankingApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionFactory, ConnectionFactory>();
        return services;
    }

    public static IServiceCollection AddDomain(this IServiceCollection services)
    {        
        //services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
        services.AddSingleton<ICreditCardRepository, CreditCardRepository>();
        services.AddSingleton<IPaymentRepository, PaymentRepository>();
        return services;
    }
}
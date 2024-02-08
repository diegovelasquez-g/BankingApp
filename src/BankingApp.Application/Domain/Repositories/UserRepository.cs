using BankingApp.Application.Domain.Entities;
using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Infraestructure.Persistance;
using Dapper;
using System.Data;

namespace BankingApp.Application.Domain.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _dapperContext;

    public UserRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<bool> AddAsync(User user)
    {
        var connection = _dapperContext.Connection;
        if (connection is null)
            throw new Exception("Connection is null");


        var query = "spCreateUser";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", user.UserId);
        parameters.Add("Username", user.Username);
        parameters.Add("Email", user.Email);
        parameters.Add("Password", user.Password);
        parameters.Add("FirstName", user.FirstName);
        parameters.Add("LastName", user.LastName);

        var recordsAffected = await connection.ExecuteAsync(query, parameters, _dapperContext.Transaction, commandType: CommandType.StoredProcedure);
        return recordsAffected > 0;

    }

    public async Task<User?> AuthUserAsync(string email, string password)
    {
        var connection = _dapperContext.Connection;
        if (connection is null)
            throw new Exception("Connection is null");


        var query = "spAuthUser";
        var parameters = new DynamicParameters();
        parameters.Add("Email", email);
        parameters.Add("Password", password);

        var user = await connection.QuerySingleOrDefaultAsync<User>(query, parameters, commandType: CommandType.StoredProcedure);
        return user;

    }

    public async Task<User?> GeyByEmailAsync(string email)
    {
        var connection = _dapperContext.Connection;
        if (connection is null)
            throw new Exception("Connection is null");


        var query = "spGetUserByEmail";
        var parameters = new DynamicParameters();
        parameters.Add("Email", email);

        var user = await connection.QuerySingleOrDefaultAsync<User>(query, parameters, _dapperContext.Transaction, commandType: CommandType.StoredProcedure);
        return user;

    }
}

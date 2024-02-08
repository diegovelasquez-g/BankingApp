using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BankingApp.Application.Infraestructure.Persistance;

public sealed class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("sqlConnection");
    }

    private readonly string? _connectionString;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public IDbConnection Connection
    {
        get
        {
            if (_connection is null || _connection.State != ConnectionState.Open)
                _connection = new SqlConnection(_connectionString);
            return _connection;
        }
    }

    public IDbTransaction? Transaction
    {
        get => _transaction;
        set => _transaction = value;
    }
}

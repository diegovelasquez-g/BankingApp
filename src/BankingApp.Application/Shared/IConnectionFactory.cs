using System.Data;

namespace BankingApp.Application.Shared;

public interface IConnectionFactory
{
    IDbConnection? Connection { get; }
    IDbTransaction? Transaction { get; set; }
}

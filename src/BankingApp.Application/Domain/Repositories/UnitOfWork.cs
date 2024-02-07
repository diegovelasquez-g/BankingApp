using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Shared;

namespace BankingApp.Application.Domain.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly IConnectionFactory _connectionFactory;
    public IUserRepository Users { get; private set; }

    public UnitOfWork(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        Init();
    }

    private void Init()
    {
        Users = new UserRepository(_connectionFactory);
    }

    public void BeginTransaction()
    {
        var connection = _connectionFactory.Connection;
        connection.Open();
        _connectionFactory.Transaction = connection.BeginTransaction();
    }

    public void Commit()
    {
        _connectionFactory.Transaction?.Commit();
        _connectionFactory.Transaction?.Dispose();
        _connectionFactory.Transaction = null;
    }

    public void Rollback()
    {
        _connectionFactory.Transaction?.Rollback();
        _connectionFactory.Transaction?.Dispose();
        _connectionFactory.Transaction = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connectionFactory.Transaction?.Dispose();
                _connectionFactory.Connection?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

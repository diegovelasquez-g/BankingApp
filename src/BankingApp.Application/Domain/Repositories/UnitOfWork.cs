using BankingApp.Application.Domain.Interfaces;
using BankingApp.Application.Infraestructure.Persistance;
using BankingApp.Application.Shared;

namespace BankingApp.Application.Domain.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private readonly DapperContext _dapperContext;
    public IUserRepository Users { get; private set; }

    public UnitOfWork(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
        Init();
    }

    private void Init()
    {
        Users = new UserRepository(_dapperContext);
    }

    public void BeginTransaction()
    {
        var connection = _dapperContext.Connection;
        connection.Open();
        _dapperContext.Transaction = connection.BeginTransaction();
    }

    public void Commit()
    {
        _dapperContext.Transaction?.Commit();
        _dapperContext.Transaction?.Dispose();
        _dapperContext.Transaction = null;
    }

    public void Rollback()
    {
        _dapperContext.Transaction?.Rollback();
        _dapperContext.Transaction?.Dispose();
        _dapperContext.Transaction = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dapperContext.Transaction?.Dispose();
                _dapperContext.Connection?.Dispose();
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

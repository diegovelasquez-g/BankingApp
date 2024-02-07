namespace BankingApp.Application.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    public IUserRepository Users { get; }
}

using System.Data;

namespace PTQ.Repositories
{
    public interface IBaseRepository
    {
        Task<IDbTransaction> BeginTransactionAsync();
    }
}
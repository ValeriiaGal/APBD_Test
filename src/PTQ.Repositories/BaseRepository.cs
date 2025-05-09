using System.Data;
using PTQ.Repositories;

public class BaseRepository : IBaseRepository
{
    protected readonly IDbConnection _connection;
    
    public BaseRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        return _connection.BeginTransaction();
    }
}
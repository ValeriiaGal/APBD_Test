using System.Data;
using DTO;
using PTQ.Repositories;

public class TeacherRepository : BaseRepository, ITeacherRepository
{
    public TeacherRepository(IDbConnection connection) : base(connection) {}

    public async Task<PotatoTeacher> GetOrCreateAsync(string name)
    {
        using var transaction = await BeginTransactionAsync();
        try
        {
            var sql = "SELECT * FROM PotatoTeacher WHERE Name = @Name";
            var existingTeacher = await _connection.QueryFirstOrDefaultAsync<PotatoTeacher>(sql, new { Name = name }, transaction);
            
            if (existingTeacher != null)
            {
                transaction.Commit();
                return existingTeacher;
            }
            
            sql = "INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)";
            var id = await _connection.ExecuteScalarAsync<int>(sql, new { Name = name }, transaction);
            
            transaction.Commit();
            return new PotatoTeacher { Id = id, Name = name };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<PotatoTeacher> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM PotatoTeacher WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<PotatoTeacher>(sql, new { Id = id });
    }
}
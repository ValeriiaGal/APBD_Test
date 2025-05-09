using System.Data;
using DTO;

namespace PTQ.Repositories;

public class QuizRepository : BaseRepository, IQuizRepository
{
    public QuizRepository(IDbConnection connection) : base(connection) {}

    public async Task<IEnumerable<Quiz>> GetAllAsync()
    {
        var sql = "SELECT Id, Name, PotatoTeacherId, PathFile FROM Quiz";
        return await _connection.QueryAsync<Quiz>(sql);
    }

    public async Task<Quiz> GetByIdAsync(int id)
    {
        var sql = "SELECT Id, Name, PotatoTeacherId, PathFile FROM Quiz WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Quiz>(sql, new { Id = id });
    }

    public async Task AddAsync(Quiz quiz)
    {
        var sql = @"INSERT INTO Quiz (Name, PotatoTeacherId, PathFile) 
                   OUTPUT INSERTED.Id 
                   VALUES (@Name, @PotatoTeacherId, @PathFile)";
        quiz.Id = await _connection.ExecuteScalarAsync<int>(sql, quiz);
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        var sql = @"UPDATE Quiz SET 
                   Name = @Name, 
                   PotatoTeacherId = @PotatoTeacherId, 
                   PathFile = @PathFile 
                   WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, quiz);
    }

    public async Task DeleteAsync(int id)
    {
        var sql = "DELETE FROM Quiz WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task SaveChangesAsync()
    {
        await Task.CompletedTask;
    }
}
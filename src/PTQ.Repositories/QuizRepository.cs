using System.Data;
using DTO;

namespace PTQ.Repositories;

public class QuizRepository : BaseRepository, IQuizRepository
{
    public QuizRepository(IDbConnection connection) : base(connection)
    {
    }

    public Task<IEnumerable<Quiz>> GetAllAsync()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, PotatoTeacherId, PathFile FROM Quiz";

        var reader = command.ExecuteReader();
        var quizzes = new List<Quiz>();

        while (reader.Read())
        {
            quizzes.Add(new Quiz
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                PotatoTeacherId = reader.GetInt32(2),
                PathFile = reader.GetString(3)
            });
        }

        return Task.FromResult<IEnumerable<Quiz>>(quizzes);
    }

    public async Task<Quiz?> GetByIdAsync(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, PotatoTeacherId, PathFile FROM Quiz WHERE Id = @Id";

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = id;
        command.Parameters.Add(parameter);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Quiz
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                PotatoTeacherId = reader.GetInt32(2),
                PathFile = reader.GetString(3)
            };
        }

        return null;
    }

    public async Task AddAsync(Quiz quiz)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Quiz (Name, PotatoTeacherId, PathFile) 
            VALUES (@Name, @PotatoTeacherId, @PathFile);
            SELECT CAST(SCOPE_IDENTITY() AS INT)";

        var nameParam = command.CreateParameter();
        nameParam.ParameterName = "@Name";
        nameParam.Value = quiz.Name;
        command.Parameters.Add(nameParam);

        var teacherIdParam = command.CreateParameter();
        teacherIdParam.ParameterName = "@PotatoTeacherId";
        teacherIdParam.Value = quiz.PotatoTeacherId;
        command.Parameters.Add(teacherIdParam);

        var pathFileParam = command.CreateParameter();
        pathFileParam.ParameterName = "@PathFile";
        pathFileParam.Value = quiz.PathFile;
        command.Parameters.Add(pathFileParam);

        object? result = command.ExecuteScalar();
        if (result != null)
        {
            quiz.Id = Convert.ToInt32(result);
        }
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = @"
            UPDATE Quiz SET 
            Name = @Name, 
            PotatoTeacherId = @PotatoTeacherId, 
            PathFile = @PathFile 
            WHERE Id = @Id";

        var idParam = command.CreateParameter();
        idParam.ParameterName = "@Id";
        idParam.Value = quiz.Id;
        command.Parameters.Add(idParam);

        var nameParam = command.CreateParameter();
        nameParam.ParameterName = "@Name";
        nameParam.Value = quiz.Name;
        command.Parameters.Add(nameParam);

        var teacherIdParam = command.CreateParameter();
        teacherIdParam.ParameterName = "@PotatoTeacherId";
        teacherIdParam.Value = quiz.PotatoTeacherId;
        command.Parameters.Add(teacherIdParam);

        var pathFileParam = command.CreateParameter();
        pathFileParam.ParameterName = "@PathFile";
        pathFileParam.Value = quiz.PathFile;
        command.Parameters.Add(pathFileParam);

        command.ExecuteNonQuery();
    }

    public async Task DeleteAsync(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Quiz WHERE Id = @Id";

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = id;
        command.Parameters.Add(parameter);

        command.ExecuteNonQuery();
    }

    public async Task SaveChangesAsync()
    {
        await Task.CompletedTask;
    }
}
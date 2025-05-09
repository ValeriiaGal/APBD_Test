using System.Data;
using DTO;
using PTQ.Repositories;

public class TeacherRepository : BaseRepository, ITeacherRepository
{
    public TeacherRepository(IDbConnection connection) : base(connection) {}

    public async Task<PotatoTeacher> GetOrCreateAsync(string name)
    {
        using var transaction = _connection.BeginTransaction();

        try
        {
            using var selectCommand = _connection.CreateCommand();
            selectCommand.Transaction = transaction;
            selectCommand.CommandText = "SELECT Id, Name FROM PotatoTeacher WHERE Name = @Name";

            var nameParam = selectCommand.CreateParameter();
            nameParam.ParameterName = "@Name";
            nameParam.Value = name;
            selectCommand.Parameters.Add(nameParam);

            using var reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                var existing = new PotatoTeacher
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };

                transaction.Commit();
                return existing;
            }
            reader.Close();

            using var insertCommand = _connection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = @"
                INSERT INTO PotatoTeacher (Name) 
                VALUES (@Name); 
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var insertNameParam = insertCommand.CreateParameter();
            insertNameParam.ParameterName = "@Name";
            insertNameParam.Value = name;
            insertCommand.Parameters.Add(insertNameParam);

            var result = insertCommand.ExecuteScalar();
            int newId = Convert.ToInt32(result);

            transaction.Commit();
            return new PotatoTeacher { Id = newId, Name = name };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<PotatoTeacher?> GetByIdAsync(int id)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM PotatoTeacher WHERE Id = @Id";

        var param = command.CreateParameter();
        param.ParameterName = "@Id";
        param.Value = id;
        command.Parameters.Add(param);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new PotatoTeacher
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }

        return null;
    }
}

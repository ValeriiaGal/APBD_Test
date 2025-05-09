using DTO;

namespace PTQ.Repositories;

public interface IPotatoTeacherRepository
{
    Task<PotatoTeacher> GetOrCreateTeacherAsync(string name);
    // Other repository methods
}
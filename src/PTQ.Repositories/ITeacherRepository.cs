using DTO;

namespace PTQ.Repositories
{
    public interface ITeacherRepository : IBaseRepository
    {
        Task<PotatoTeacher> GetOrCreateAsync(string name);
        Task<PotatoTeacher> GetByIdAsync(int id);
    }
}
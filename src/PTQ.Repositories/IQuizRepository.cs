using DTO;

namespace PTQ.Repositories
{
    public interface IQuizRepository : IBaseRepository
    {
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task<Quiz> GetByIdAsync(int id);
        Task AddAsync(Quiz quiz);
        Task UpdateAsync(Quiz quiz);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
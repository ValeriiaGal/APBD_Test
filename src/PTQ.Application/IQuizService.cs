namespace PTQ.Application;

public interface IQuizService
{
    Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
    Task<QuizDto> GetQuizByIdAsync(int id);
    Task<int> CreateQuizAsync(CreateQuizDto createQuizDto);
    Task<bool> DeleteQuizAsync(int id);
}
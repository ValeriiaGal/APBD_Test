using PTQ.Application;
public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly ITeacherRepository _teacherRepository;

    public QuizService(IQuizRepository quizRepository, ITeacherRepository teacherRepository)
    {
        _quizRepository = quizRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
    {
        var quizzes = await _quizRepository.GetAllAsync();
        var result = new List<QuizDto>();
        
        foreach (var quiz in quizzes)
        {
            var teacher = await _teacherRepository.GetByIdAsync(quiz.PotatoTeacherId);
            result.Add(new QuizDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                TeacherName = teacher?.Name,
                PathFile = quiz.PathFile
            });
        }
        
        return result;
    }

    public async Task<QuizDto> GetQuizByIdAsync(int id)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        if (quiz == null) return null;
        
        var teacher = await _teacherRepository.GetByIdAsync(quiz.PotatoTeacherId);
        
        return new QuizDto
        {
            Id = quiz.Id,
            Name = quiz.Name,
            TeacherName = teacher?.Name,
            PathFile = quiz.PathFile
        };
    }

    public async Task<int> CreateQuizAsync(CreateQuizDto createQuizDto)
    {
        using var transaction = await _quizRepository.BeginTransactionAsync();
        
        try
        {
            var teacher = await _teacherRepository.GetOrCreateAsync(createQuizDto.TeacherName);
            
            var quiz = new Quiz
            {
                Name = createQuizDto.QuizName,
                PotatoTeacherId = teacher.Id,
                PathFile = createQuizDto.PathFile
            };

            await _quizRepository.AddAsync(quiz);
            
            transaction.Commit();
            return quiz.Id;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteQuizAsync(int id)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        if (quiz == null) return false;

        await _quizRepository.DeleteAsync(id);
        return true;
    }
}
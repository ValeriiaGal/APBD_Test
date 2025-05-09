using Microsoft.AspNetCore.Mvc;
using PTQ.Application;

[ApiController]
[Route("api/quizzes")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    // GET: api/quizzes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetAllQuizzes()
    {
        try
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: api/quizzes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        try
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            
            if (quiz == null)
            {
                return NotFound();
            }

            return Ok(quiz);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/quizzes
    [HttpPost]
    public async Task<ActionResult<int>> CreateQuiz([FromBody] CreateQuizDto createQuizDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quizId = await _quizService.CreateQuizAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuiz), new { id = quizId }, quizId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}
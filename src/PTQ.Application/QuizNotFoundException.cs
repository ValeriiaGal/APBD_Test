namespace PTQ.Application;

public class QuizNotFoundException : Exception
{
    public QuizNotFoundException(int quizId) 
        : base($"Quiz with ID {quizId} was not found.")
    {
    }
}
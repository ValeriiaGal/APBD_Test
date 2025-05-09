using System.ComponentModel.DataAnnotations;

namespace PTQ.Application;

public class CreateQuizDto
{
    [Required]
    [StringLength(100)]
    public string QuizName { get; set; }

    [Required]
    [StringLength(100)]
    public string TeacherName { get; set; }

    [Required]
    public string PathFile { get; set; }
}
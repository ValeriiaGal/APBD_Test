using System.Data;
using Microsoft.Data.SqlClient;
using PTQ.Application;
using PTQ.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PotatoTeacherDatabase");
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<IQuizRepository, QuizRepository>();
builder.Services.AddTransient<IQuizService, QuizService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
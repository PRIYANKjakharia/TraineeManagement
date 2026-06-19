using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TraineeManagement.Api.Middleware;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.AddScoped<ITraineeService , TraineeService>();
builder.Services.AddScoped<IMentorService , MentorService>();
builder.Services.AddScoped<ILearningTaskService , LearningTaskService>();
builder.Services.AddScoped<ITaskAssignmentService , TaskAssignmentService>();
builder.Services.AddScoped<IAuthService , AuthService>();
builder.Services.AddScoped<ISubmissionService , SubmissionService>();
builder.Services.AddScoped<IReviewService , ReviewService>();
builder.Services.AddScoped<IFileStorageService , LocalFileStorageService>();
builder.Services.AddScoped<ISubmissionFileService , SubmissionFileService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);
});

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme ).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();
// Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("Admin@123"));

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,policy  => {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);

app.Run();


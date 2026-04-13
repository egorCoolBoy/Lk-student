using Microsoft.EntityFrameworkCore;
using UsersService.Infrastructure.AppDbContext;
using FluentValidation;
using FluentValidation.AspNetCore;
using UsersService;
using UsersService.Application.Hash;
using UsersService.Application.Users;
using UsersService.Infrastructure.Hash;
using UsersService.Presentation.ExceptionMiddleware;
using UsersService.Presentation.Validators;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

builder.Services.AddScoped<IUsersService, UserService >();
builder.Services.AddScoped<IPasswordHash, PasswordHash>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))); 

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
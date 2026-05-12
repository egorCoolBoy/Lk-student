using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.FileStorage;
using ProfileDocksService.Applicantion.Options;
using ProfileDocksService.Infrastructure.AppDbContext;
using ProfileDocksService.Presentation.Policy.CanView;
using UsersService.Presentation.ExceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IPassportService, PassportService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanView", policy =>
    {
        policy.Requirements.Add(new CanViewRequirement());
    });
});
builder.Services.Configure<MinioOptions>(
    builder.Configuration.GetSection("Minio"));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])
            )
        };
    });
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
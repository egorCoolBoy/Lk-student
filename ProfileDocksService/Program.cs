using System.Text;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfileDocksService.Applicantion;
using ProfileDocksService.Applicantion.EducationDocks;
using ProfileDocksService.Applicantion.EducationScans;
using ProfileDocksService.Applicantion.FileStorage;
using ProfileDocksService.Applicantion.Interface;
using ProfileDocksService.Applicantion.Options;
using ProfileDocksService.Applicantion.PassportDocks;
using ProfileDocksService.Applicantion.PassportScans;
using ProfileDocksService.Infrastructure.AppDbContext;
using ProfileDocksService.Infrastructure.Consumers;
using ProfileDocksService.Presentation.Implementations;
using ProfileDocksService.Presentation.Options;
using UsersService.Presentation.ExceptionMiddleware;
using Contracts;

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
builder.Services.AddScoped<IPassportScanService, PassportScanService>();
builder.Services.AddScoped<IEducationScanService, EducationScanService>();

builder.Services.Configure<DirApi>(builder.Configuration.GetSection("DirApi"));
builder.Services.AddHttpClient<IDirectoriesAPI,DirectoriesAPI>( (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<DirApi>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ApplicantCreatedConsumer>();
    

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");

        cfg.ReceiveEndpoint("applicant-created-queue", e =>
        {
            e.ConfigureConsumer<ApplicantCreatedConsumer>(context);
        });
    });
});
builder.Services.Configure<MinioOptions>(
    builder.Configuration.GetSection("Minio"));



builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Secret)
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
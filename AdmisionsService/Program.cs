using System.Text;
using AdmisionsService.Application;
using AdmisionsService.Application.Interfaces;
using AdmisionsService.Application.ManagerFacultyService;
using AdmisionsService.Infrastructure;
using AdmisionsService.Infrastructure.AppDbContext;
using AdmisionsService.Infrastructure.Implementations;
using AdmisionsService.Presentation.ExecptionMiddleware;
using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IManagerFacultyService, ManagerFacultyService>();
builder.Services.AddScoped<IAdmissionService, AdmissionService>();

builder.Services.Configure<NOptions>(builder.Configuration.GetSection("NOptions"));
builder.Services.Configure<DocksApi>(builder.Configuration.GetSection("DocksApi"));
builder.Services.AddHttpClient<IDocumentAPI, DocumentAPI>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<DocksApi>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.Configure<DirApi>(builder.Configuration.GetSection("DirApi"));
builder.Services.AddHttpClient<IDirectoriesAPI,DirectoriesAPI>( (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<DirApi>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});
builder.Services.Configure<UsersApi>(builder.Configuration.GetSection("UsersApi"));
builder.Services.AddHttpClient<IUsersServiceApi, UsersServiceApi>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<UsersApi>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMassTransit(x =>
{
    
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });
    
    x.UsingRabbitMq((context, cfg) =>
    { 
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
       
    });
    
});

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
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using Microsoft.Extensions.Options;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<UsersApiOption>(builder.Configuration.GetSection("UsersApi"));
builder.Services.AddHttpClient<IUsersServiceApi, UsersServiceApi>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<UsersApiOption>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.Configure<DirApi>(builder.Configuration.GetSection("DirApi"));
builder.Services.AddHttpClient<IDirectoriesApi,DirectoriesApi>( (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<DirApi>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Directory}/{action=GetImportedData}");

app.Run();
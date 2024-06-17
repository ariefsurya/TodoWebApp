using CurrieTechnologies.Razor.SweetAlert2;
using TodoWebApp.Data;
using TodoWebApp.Repository.TodoRepository;
using TodoWebApp.Repository.UserRepository;
using TodoWebApp.Service;
//using Blazored.Modal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("CustomHttpClient").ConfigurePrimaryHttpMessageHandler(() => new CustomHttpClientHandler());
//builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITodoRepository, TodoRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICookieService, CookieService>();
builder.Services.AddTransient<StringService>();
builder.Services.AddSweetAlert2();
builder.Services.AddBlazorBootstrap();
//builder.Services.AddBlazoredModal();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
public class CustomHttpClientHandler : HttpClientHandler
{
    public CustomHttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    }
}
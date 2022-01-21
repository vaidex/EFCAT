using Sample.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Configuration;
using Blazored.LocalStorage;
using Sample.Server.Services;
using EFCAT.Service.Storage;
using EFCAT.Service.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ITestAsyncRepository, TestAsyncRepository>();

builder.Services.AddDbContext<TestDbContext>(
    options => options
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new System.Version("8.0.25")))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient();
builder.Services.AddLocalStorage();
builder.Services.AddAuthenticationService<MyAuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
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

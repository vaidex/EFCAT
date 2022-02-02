using Sample.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Sample.Model.Configuration;
using Sample.Server.Services;
using EFCAT.Service.Storage;
using EFCAT.Service.Authentication;
using Sample.Model.Entity;

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

builder.Services.AddHttpClient();
builder.Services.AddLocalStorage();
builder.Services.AddAuthenticationService<MyAuthenticationService>();

var app = builder.Build();

using (var context = app.Services.CreateScope().ServiceProvider.GetService<TestDbContext>()) {
    if (context == null) return;
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    context.Set<User>().Add(new User() {
        Name = "Nico",
        Email = "tel@mail.com",
        Password = "test",
        Codes = new[] {
            new EmailVerificationCode() { Value = "fromcodes" }
        }
    });
    context.SaveChanges();
}

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

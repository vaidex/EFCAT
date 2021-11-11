using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITestRepository, TestRepository>();

builder.Services.AddDbContext<TestDbContext>(
    options => options
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new System.Version("8.0.25")))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

var app = builder.Build();

using (var context = app.Services.CreateScope().ServiceProvider.GetService<TestDbContext>()) {
    if (context == null) return;
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

/*
using(var context = app.Services.CreateScope().ServiceProvider.GetService<TestDbContext>()) {
    if (context == null) return;
    context.Database.Migrate();

    if(context.Find<TestEntity>() == null) {
        var iu = context.Add<TestEntity>(new TestEntity() {
            Name = "TestName",
            Password = "TestPassword",
            Number = 2.23
        });
        context.SaveChanges();
    }
}*/

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

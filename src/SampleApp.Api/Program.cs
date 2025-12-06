using Microsoft.EntityFrameworkCore;
using BasicApp.Infrastructure.DI;
using BasicApp.Infrastructure.Persistence;
using BasicApp.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (InMemory for starter app)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BasicAppDb"));

// Reflection-based registration of repositories/services from Infrastructure
builder.Services.AddInfrastructure();

var app = builder.Build();

// Seed some data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob",   Email = "bob@example.com" }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

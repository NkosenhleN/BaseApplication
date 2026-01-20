using Base.API.Validators;
using Base.Application.Interfaces;
using Base.Application.Services;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Infrastructure.Data;
using Base.Infrastructure.Persistence;
using Base.Infrastructure.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<BaseDbContext>(options =>
        options.UseInMemoryDatabase("DevDb")); 
}
else
{
    builder.Services.AddDbContext<BaseDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
}
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

    if (!context.Users.Any())
    {
        var hasher = new PasswordHasher();
        var (hash, salt) = hasher.HashPassword("Admin123!");

        context.Users.Add(new User("admin", "System", "Admin", "admin@example.com", hash, salt));
        context.Users.Add(new User("user1", "John", "Doe", "john@example.com", hash, salt));
        context.SaveChanges();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


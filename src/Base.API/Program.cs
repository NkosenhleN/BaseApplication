using Base.API.Validators;
using Base.Application.Interfaces;
using Base.Application.Services;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Infrastructure.Data;
using Base.Infrastructure.Data.Seed;
using Base.Infrastructure.Repositories;
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
    //builder.Services.AddDbContext<BaseDbContext>(options =>
    //    options.UseInMemoryDatabase("DevDb"));

    builder.Services.AddDbContext<BaseDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
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
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await RoleSeeder.SeedAsync(context);
        await AdminUserSeeder.SeedAsync(context, passwordHasher);
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


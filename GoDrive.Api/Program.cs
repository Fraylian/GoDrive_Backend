using Dominio.Entidades;
using Persistencia.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProyectoContext>(options =>
{
    options.UseSqlServer("name=DefaultConnection");
});

//var UserBuilder = builder.Services.AddIdentityCore<Usuarios>();
//var identityBuilder = new IdentityBuilder(UserBuilder.UserType, UserBuilder.Services);
//identityBuilder.AddEntityFrameworkStores<ProyectoContext>();
//identityBuilder.AddSignInManager<SignInManager<Usuarios>>();

var app = builder.Build();

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

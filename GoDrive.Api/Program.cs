using Dominio.Entidades;
using Persistencia.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Aplicacion.Metodos.Vehiculo;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Aplicacion.Seguridad;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Go Drive Api ", Version = "v1" });
    c.CustomSchemaIds(x => x.FullName);
});
builder.Services.AddScoped<IPasswordHasher<Clientes>, PasswordHasher<Clientes>>();

builder.Services.AddDbContext<ProyectoContext>(options =>
{
    options.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddMediatR(typeof(listado.Manejador).Assembly);
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Insertar>());

var UserBuilder = builder.Services.AddIdentityCore<Usuarios>();
var identityBuilder = new IdentityBuilder(UserBuilder.UserType, UserBuilder.Services);
identityBuilder.AddEntityFrameworkStores<ProyectoContext>();
identityBuilder.AddSignInManager<SignInManager<Usuarios>>();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
builder.Services.AddCors(o => o.AddPolicy("corsApp", builder => {
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ProyectoContext>();
    var userManager = services.GetRequiredService<UserManager<Usuarios>>();
    var dataPrueba = new DataPrueba();
    await dataPrueba.InsertarUsuario(context, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

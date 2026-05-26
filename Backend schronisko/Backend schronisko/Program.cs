using Backend_schronisko.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja SchroniskoContext z użyciem MSSQL
builder.Services.AddDbContext<SchroniskoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllers();
// Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:5173") // Dokładny adres serwera Vue
                         .AllowAnyHeader()  // Pozwala na dowolne nagłówki
                         .AllowAnyMethod(); // Pozwala na GET, POST, PUT, DELETE itd.
        });
});

builder.Services.AddOpenApi();
// 1. Rejestracja Swaggera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Aktywacja Swaggera w trybie programisty
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();

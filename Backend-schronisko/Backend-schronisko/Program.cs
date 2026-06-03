using Backend_schronisko.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja SchroniskoContext z użyciem MSSQL
builder.Services.AddDbContext<SchroniskoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignoruje zapętlenia relacji podczas generowania JSON-a
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:5174") // dozwolone adresy
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

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SchroniskoContext>();
    dbContext.Database.Migrate(); // Automatycznie tworzy tabele na podstawie migracji
}

app.Run();
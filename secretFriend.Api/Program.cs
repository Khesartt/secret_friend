using secretFriend.Api.Application;
using secretFriend.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configurar capas de la aplicación
builder.Services.AddApplication();

// Configurar infraestructura (MongoDB)
builder.Services.AddInfrastructure(builder.Configuration);

// Configurar logging
builder.Services.AddLogging();

// Configurar CORS si es necesario
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAll");

// Mapear controladores
app.MapControllers();

// Endpoint de bienvenida
app.MapGet("/", () => new
{
    message = "¡Bienvenido al API de Amigo Secreto!",
    documentation = "/openapi/v1.json",
    health = "/api/health",
    info = "/api/info",
    games = "/api/games",
    timestamp = DateTime.UtcNow
})
.WithName("Welcome")
.WithOpenApi();

app.Run();

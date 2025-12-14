using secretFriend.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registrar servicios de dependencias
builder.Services.AddSingleton<Random>();
builder.Services.AddScoped<ISecretFriendService, SecretFriendService>();

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
    health = "/api/secretfriend/health",
    info = "/api/secretfriend/info",
    timestamp = DateTime.UtcNow
})
.WithName("Welcome")
.WithOpenApi();

app.Run();

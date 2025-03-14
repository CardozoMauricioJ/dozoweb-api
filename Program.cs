using DozoWeb.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura la cadena de conexión para el DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
//builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer(); // Para habilitar Swagger
builder.Services.AddSwaggerGen(); // Configuración de Swagger

var app = builder.Build();

// Configura el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita Swagger en modo desarrollo
    app.UseSwaggerUI(); // Interfaz de usuario para Swagger
}


app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




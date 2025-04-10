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

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // URL del frontend
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithExposedHeaders("X-Total-Count", "X-Total-Pages", "X-Current-Page", "X-Page-Size");
        });
});


/*{
    throw new NotImplementedException();
}*/

var app = builder.Build();

// Configura el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita Swagger en modo desarrollo
    app.UseSwaggerUI(); // Interfaz de usuario para Swagger
}

//app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();




using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GymPlannerApi.Data;
using GymPlannerApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2️⃣ Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=gymplanner.db"));

// 3️⃣ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 4️⃣ JWT Authentication
var key = builder.Configuration["Jwt:Key"] ?? "VERY_LONG_SECRET_KEY_1234567890123456"; // 32+ chars
var keyBytes = Encoding.UTF8.GetBytes(key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// 5️⃣ Middleware
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.Urls.Add("http://localhost:5000");

// 6️⃣ Crear DB y usuarios de prueba si no existen
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.EnsureCreated();

    if (!ctx.Users.Any())
    {
        // Crear usuarios de prueba con PasswordHash
        ctx.Users.AddRange(
            new User
            {
                Username = "admin",
                Role = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Nombre = "Admin",
                Apellido = "User",
                DNI = "00000000",
                Telefono = "123456789",
                Direccion = "Calle Admin 1"
            },
            new User
            {
                Username = "profe",
                Role = "profe",
                Email = "profe@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("profe123"),
                Nombre = "Profesor",
                Apellido = "User",
                DNI = "11111111",
                Telefono = "123456789",
                Direccion = "Calle Profe 1"
            },
            new User
            {
                Username = "alumno",
                Role = "alumno",
                Email = "alumno@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("alumno123"),
                Nombre = "Alumno",
                Apellido = "User",
                DNI = "22222222",
                Telefono = "123456789",
                Direccion = "Calle Alumno 1"
            }
        );
        ctx.SaveChanges();
    }
}

// 7️⃣ Swagger y routing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

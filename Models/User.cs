using System.ComponentModel.DataAnnotations;
namespace GymPlannerApi.Models;


public class User
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }    
    public string Role { get; set; } // admin, pro, alumno
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string DNI { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }    
    public bool IsEnabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
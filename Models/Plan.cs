using System.ComponentModel.DataAnnotations;
namespace GymPlannerApi.Models;
public class Plan {
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public Guid StudentId { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string Notes { get; set; }
}
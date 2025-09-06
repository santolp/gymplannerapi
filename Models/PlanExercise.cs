using System.ComponentModel.DataAnnotations;
namespace GymPlannerApi.Models;
public class PlanExercise {
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlanId { get; set; }
    public Guid ExerciseTemplateId { get; set; }
    public int WeekNumber { get; set; }
    public int? Sets { get; set; }
    public string Reps { get; set; }
    public bool IsOverride { get; set; } = false;
    public string Day { get; set; }
}
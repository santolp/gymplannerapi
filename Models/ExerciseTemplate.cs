using System.ComponentModel.DataAnnotations;
namespace GymPlannerApi.Models;
public class ExerciseTemplate {
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Zone { get; set; }
    public string SeriesType { get; set; }
    public int? DefaultSets { get; set; }
    public string DefaultReps { get; set; }
}
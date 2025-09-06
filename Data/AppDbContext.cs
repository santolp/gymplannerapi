using Microsoft.EntityFrameworkCore;
using GymPlannerApi.Models;
namespace GymPlannerApi.Data;
public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<ExerciseTemplate> ExerciseTemplates { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanExercise> PlanExercises { get; set; }
}
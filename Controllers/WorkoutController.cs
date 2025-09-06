using Microsoft.AspNetCore.Mvc;
using GymPlannerApi.Data;
using Microsoft.EntityFrameworkCore;
namespace GymPlannerApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class WorkoutController : ControllerBase {
    private readonly AppDbContext _ctx;
    public WorkoutController(AppDbContext ctx){ _ctx = ctx; }
    [HttpGet("day")]
    public async Task<IActionResult> Day([FromQuery] Guid userId, [FromQuery] DateTime date){
        var plan = await _ctx.Plans.FirstOrDefaultAsync(p => p.StudentId==userId && p.IsActive);
        if (plan==null) return NotFound("No plan");
        var exercises = await _ctx.PlanExercises.Where(pe=>pe.PlanId==plan.Id).ToListAsync();
        var list = new List<object>();
        foreach(var pe in exercises) {
            var tmpl = await _ctx.ExerciseTemplates.FindAsync(pe.ExerciseTemplateId);
            list.Add(new { pe.Id, name = tmpl?.Name ?? "Ej", pe.WeekNumber, pe.Day, pe.Sets, pe.Reps });
        }
        return Ok(new { planId = plan.Id, planName = plan.Name, exercises = list });
    }
}
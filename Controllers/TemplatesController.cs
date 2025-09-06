using Microsoft.AspNetCore.Mvc;
using GymPlannerApi.Data;
using GymPlannerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GymPlannerApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase {
    private readonly AppDbContext _ctx;
    public TemplatesController(AppDbContext ctx){ _ctx = ctx; }
    [HttpGet] public async Task<IActionResult> GetAll(){ return Ok(await _ctx.ExerciseTemplates.ToListAsync()); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] ExerciseTemplate t){ _ctx.ExerciseTemplates.Add(t); await _ctx.SaveChangesAsync(); return CreatedAtAction(nameof(GetAll), t); }
}
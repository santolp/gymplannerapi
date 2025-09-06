using Microsoft.AspNetCore.Mvc;
using GymPlannerApi.Data;
using GymPlannerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GymPlannerApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlansController : ControllerBase {
    private readonly AppDbContext _ctx;
    public PlansController(AppDbContext ctx){ _ctx = ctx; }
    [HttpGet] public async Task<IActionResult> GetAll(){ return Ok(await _ctx.Plans.ToListAsync()); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] Plan p){ _ctx.Plans.Add(p); await _ctx.SaveChangesAsync(); return CreatedAtAction(nameof(GetAll), p); }
}
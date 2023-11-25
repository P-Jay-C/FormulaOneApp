using FormulaOneApp.Data;
using FormulaOneApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormulaOneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        public TeamsController(AppDbContext context)
        {
            this._context = context;
        }

        //private static List<Team> teams = new List<Team>()
        //{
        //    new Team() { Id = 1,Country="Germany",Name="Mercedes AMG F1", TeamPrinciple="Toto Wolf"},
        //    new Team() { Id = 2,Country="Italy", Name="Ferrari", TeamPrinciple="Mattia"},
        //    new Team() { Id = 3,Country="Swiss", Name="Alpha Romeo", TeamPrinciple="Freideric Vasseur"}
        ////};

        private readonly AppDbContext _context;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var teams = await _context.Teams.ToListAsync();

            return Ok(teams);
        }

        [HttpGet(template: "{id:int}")]
        public async Task<IActionResult> Get(int id) {

            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if(team == null)
            {
                return BadRequest("Invalid Id");
            }

            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Team team)
        {
            if(team != null)
            {
               await _context.Teams.AddAsync(team);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(Get), new {id = team?.Id }, team);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(int i, string country)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == i);


            if (team == null)
            {
                return BadRequest("Invalid Id");
            }

            team.Country = country;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

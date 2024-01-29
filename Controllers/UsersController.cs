using IsMyPlantSickApp.Database;
using IsMyPlantSickApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IsMyPlantSickApp.Controllers;

[ApiController]
[Route("[users]")]
public class UsersController : ControllerBase {
    private readonly ILogger<UsersController> _logger;
    private readonly AppDbContext _dbContext;

    public UsersController(ILogger<UsersController> logger, AppDbContext dbContext) {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]  // TODO maybe: Add caching, sorting, pagination, etc.
    public async Task<ActionResult<User>> Get(int id) {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User newUser) {
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut]
    public async Task<IActionResult> Update(User user) {
        _dbContext.Entry(user).State = EntityState.Modified;

        try {
            await _dbContext.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (await _dbContext.Users.FindAsync(user.Id) == null) {
                return NotFound();
            } else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return NotFound();

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}

using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController(AppDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPerson(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return CreatedAtRoute("GetUserById", new{id = user.Id}, user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await dbContext.Users.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if(user is not null) return Ok(user);

        return NotFound();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        var userToUpdate = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (userToUpdate is null) return NotFound();

        userToUpdate.FirstName = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName : userToUpdate.FirstName;
        userToUpdate.LastName = !string.IsNullOrEmpty(user.LastName) ? user.LastName : userToUpdate.LastName;

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var userToDelete = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (userToDelete is null) return NotFound();
        dbContext.Users.Remove(userToDelete);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}

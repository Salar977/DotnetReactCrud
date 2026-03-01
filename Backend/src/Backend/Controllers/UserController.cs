using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<User>> AddUser(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdUser = await userRepository.AddUserAsync(user);

        if(createdUser is null)
            return BadRequest("Failed to create user.");

        return CreatedAtRoute("GetUserById", new{id = user.Id}, user);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return Ok(await userRepository.GetAllUsersAsync());
    }

    [HttpGet("{id:int}", Name = "GetUserById")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await userRepository.GetUserByIdAsync(id);

        if (user is not null) return Ok(user);

        return NotFound();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
    {
        var userToUpdate = await userRepository.UpdateUserAsync(id, user);
        if (userToUpdate is null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var userToDelete = await userRepository.DeleteUserAsync(id);
        if (!userToDelete) return NotFound();
        return NoContent();
    }
}

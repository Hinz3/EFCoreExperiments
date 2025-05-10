using EFCoreExperiments.Core.Interfaces.Repositories;
using EFCoreExperiments.DataContext.Contexts;
using EFCoreExperiments.DataContext.Entities;
using EFCoreExperiments.TestAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExperiments.TestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(MainContext context, IUserRepository userRepository) : ControllerBase
{
    private readonly MainContext context = context;
    private readonly IUserRepository userRepository = userRepository;

    [HttpGet()]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userRepository.GetUsers();

        return Ok(users);
    }

    [HttpPut("GenerateUsers")]
    public async Task<IActionResult> GenerateUsers(int amount)
    {
        var users = new List<User>();

        for (int i = 0; i < amount; i++)
        {
            users.Add(new User
            {
                Id = 0,
                Firstname = RandomHelper.RandomWord(10),
                Lastname = RandomHelper.RandomWord(20),
                EmailAddress = RandomHelper.RandomEmail(10),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow.AddMinutes(10)  
            });
        }

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        return Ok();
    }
}

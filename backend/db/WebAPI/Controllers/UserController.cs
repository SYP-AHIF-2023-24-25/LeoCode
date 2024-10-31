using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public UserController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateUser(string username, string firstname, string lastname, bool isTeacher)
    {
        try
        {
            User user = _unitOfWork.Users.GetByUsername(username);

            if (user == null)
            {
                _unitOfWork.Users.CreateUser(username, firstname, lastname, isTeacher);
                await _unitOfWork.SaveChangesAsync();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _unitOfWork.Users.GetAllUsers();
        return Ok(users);
    }
}

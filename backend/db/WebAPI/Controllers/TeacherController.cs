using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

[Route("api/[controller]")]
public class TeacherController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public TeacherController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateUser(string username, string firstname, string lastname)
    {
        try
        {
            Teacher user = _unitOfWork.Teacher.GetByUsername(username);

            if (user == null)
            {
                _unitOfWork.Teacher.CreateUser(username, firstname, lastname);
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
        var users = await _unitOfWork.Teacher.GetAllUsers();
        return Ok(users);
    }
}

namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

[Route("api/[controller]")]
public class AssignmentsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public AssignmentsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _unitOfWork.Assignments.GetAll();
        return Ok(assignments);
    }

    [HttpGet("OneAssignment")]
    public async Task<IActionResult> GetOneAssignment(string creator, string name)
    {
        var assignments = await _unitOfWork.Assignments.GetOneAssignment(creator,name);
        return Ok(assignments);
    }
    [HttpPost]
    public async Task<IActionResult> AddAssignmentAsync(string exerciseName, string creator, DateTime dateDue, string Name)
    {
        await _unitOfWork.Assignments.CreateAssignment(exerciseName, creator, dateDue, Name); 
        return Ok();
    }
}
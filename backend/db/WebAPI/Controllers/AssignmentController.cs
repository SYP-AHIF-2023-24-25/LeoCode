namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> GetAssignments(string? username)
    {
        var assignments = await _unitOfWork.Assignments.GetAll(username);
        return Ok(assignments);
    }

    [HttpGet("OneAssignment")]
    public async Task<IActionResult> GetOneAssignment(string creator, string name)
    {
        var assignments = await _unitOfWork.Assignments.GetOneAssignment(creator, name);
        return Ok(assignments);
    }
    [HttpPost]
    public ActionResult<string> AddAssignmentAsync(string exerciseName, string creator, DateTime dateDue, string Name)
    {
        string link = _unitOfWork.Assignments.CreateAssignment(exerciseName, creator, dateDue, Name);
        return Content(link, "text/plain");
    }

    [HttpPost("JoinAssignment")]
    public async Task<IActionResult> JoinAssignmentAsync([FromBody] JoinAssignmentRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.IfStudentName))
        {
            return BadRequest("Invalid request data.");
        }

        _unitOfWork.Assignments.JoinAssignment(request.AssignmentId, request.IfStudentName);
        return Ok();
    }

    [HttpGet("GetAssignmentUsers")]
    public async Task<IActionResult> GetAssignmentUsers(int assignmentId)
    {
        var assignmentUsers = await _unitOfWork.Assignments.GetAssignmentUsers(assignmentId);
        return Ok(assignmentUsers);
    }

    public class JoinAssignmentRequest
    {
        public int AssignmentId { get; set; }
        public string IfStudentName { get; set; }
    }
}
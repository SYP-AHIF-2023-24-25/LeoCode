namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

[Route("api/[controller]")]
public class ExerciseController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ExerciseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
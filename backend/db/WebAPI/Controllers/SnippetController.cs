namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

[Route("api/[controller]")]
public class SnippetController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public SnippetController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
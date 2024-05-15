namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

[Route("api/[controller]")]
public class ArrayOfSnippetsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ArrayOfSnippetsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
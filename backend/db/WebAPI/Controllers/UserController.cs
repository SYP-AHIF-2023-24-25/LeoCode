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
}

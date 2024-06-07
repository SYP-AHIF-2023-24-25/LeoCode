namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Dto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
public class ExerciseController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ExerciseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /*public record SnippetDto(string Code, bool ReadOnlySection, string FileName);
    public record ArrayOfSnippetsDto(SnippetDto[] snippets);
    public record ExerciseDto(string Name, string Description, Language Language, Year Year, Subject Subject, ArrayOfSnippetsDto[] arrayOfSnippets);*/

    [HttpPost]
    public async Task<IActionResult> AddExerciseAsync([FromBody] ArrayOfSnippetsDto arrayOfSnippets,string name,string description,Language language, Year year,Subject subject,string username)
    {
       User user = _unitOfWork.Users.GetByUsername(username);
       try
        {
            Exercise exercise = new Exercise
            {
                Name = name,
                Description = description,
                Language = language,
                Year = year,
                Subject = subject,
                UserId = user.Id,
                User = user
            };
            exercise.ArrayOfSnippets = new ArrayOfSnippets
            {
                Exercise = exercise,
                ExerciseId = exercise.Id
            };
            
            foreach (var snippet in arrayOfSnippets.snippets)
            {
                exercise.ArrayOfSnippets.Snippets.Add(new Snippet
                {
                    Code = snippet.Code,
                    ReadonlySection = snippet.ReadOnlySection,
                    FileName = snippet.FileName,
                    ArrayOfSnippets = exercise.ArrayOfSnippets,
                    ArrayOfSnippetsId = exercise.ArrayOfSnippets.Id
                });
            }
            
            await _unitOfWork.Exercises.AddAsync(exercise);
            await _unitOfWork.ArrayOfSnippets.AddAsync(exercise.ArrayOfSnippets);
            await _unitOfWork.Snippets.AddRangeAsync(exercise.ArrayOfSnippets.Snippets);
            await _unitOfWork.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetExersiceByUsername(string username, string? exercisenName)
    {
        try
        {
            User user = _unitOfWork.Users.GetByUsername(username);
            List<ExerciseDto> exercises = await _unitOfWork.Exercises.GetExersiceByUsernameAsync(user, exercisenName);
            return Ok(exercises);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
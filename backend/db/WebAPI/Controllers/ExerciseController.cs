namespace WebAPI.Controllers;

using Core.Contracts;
using Core.Dto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
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

    [HttpPut("UpdateDetails")]
    public async Task<IActionResult> UpdateDetails(string description, string tags, string exerciseName, string username, string newExerciseName)
    {
        string[] splitted = tags.Split(",");
        try
        {
            User user = _unitOfWork.Users.GetByUsername(username);
            List<Exercise> exercises = await _unitOfWork.Exercises.GetExersiceByUsernameAsync(user, exerciseName);
            exercises = exercises
                .Where(exercise => exerciseName == exercise.Name)
                .ToList();

            exercises[0].Description = description;
            exercises[0].Tags = splitted;
            exercises[0].Name = newExerciseName;
            exercises[0].DateUpdated = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddExerciseAsync([FromBody] ArrayOfSnippetsDto arrayOfSnippets, string name, string description, string language, string[] tags, string username, DateTime datecreated, DateTime dateupdated)
    {
        User user = _unitOfWork.Users.GetByUsername(username);
        Language enumLanguage = Language.CSharp;
        switch(language)
        {
            case "CSharp":
                enumLanguage = Language.CSharp; 
                break;
            case "Java":
                enumLanguage = Language.Java;
                break;
            case "TypeScript":
                enumLanguage = Language.TypeScript;
                break;
        }
        try
        {
            Exercise exercise = new Exercise
            {
                Name = name,
                Creator = username,
                Description = description,
                Language = enumLanguage,
                Tags = tags,
                UserId = user.Id,
                User = user,
                DateCreated = datecreated,
                DateUpdated = dateupdated
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
    public async Task<ExerciseDto[]> GetExersiceByUsername(string? username, string? exerciseName)
    {
        try
        {
            List<Exercise> exercises;
            if (username == null && exerciseName == null)
            {
                exercises = await _unitOfWork.Exercises.GetAll();
                return exercises.Select(exercise => new ExerciseDto(
                exercise.Name,
                exercise.Creator,
                exercise.Description,
                ((Language)exercise.Language).ToString(),
                exercise.Tags,

                exercise.ArrayOfSnippets.Snippets.Select(snippet => new SnippetDto(
                        snippet.Code,
                        snippet.ReadonlySection,
                        snippet.FileName)).ToArray(),
                exercise.DateCreated,
                exercise.DateUpdated

                )).ToArray();
            }
            User user = _unitOfWork.Users.GetByUsername(username);
            exercises = await _unitOfWork.Exercises.GetExersiceByUsernameAsync(user, exerciseName);
            return exercises.Select(exercise => new ExerciseDto(
            exercise.Name,
            exercise.Creator,
            exercise.Description,
            ((Language)exercise.Language).ToString(),
            exercise.Tags,
           
            exercise.ArrayOfSnippets.Snippets.Select(snippet => new SnippetDto(
                    snippet.Code,
                    snippet.ReadonlySection,
                    snippet.FileName)).ToArray(),
            exercise.DateCreated,
            exercise.DateUpdated

            )).ToArray();
        }
        catch (Exception ex)
        {
            return [];
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateExerciseForUser(string username, string description, string tags, string language, string subject, string exerciseName, [FromBody] ArrayOfSnippetsDto arrayOfSnippets, DateTime dateCreated, DateTime dateUpdated)
    {
        string[] splitted = tags.Split(",");
        try
        {
            User user = _unitOfWork.Users.GetByUsername(username);
            List<Exercise> exercises = await _unitOfWork.Exercises.GetExersiceByUsernameAsync(user, exerciseName);
            exercises = exercises
                .Where(exercise => exerciseName == exercise.Name)
                .ToList();

            if (exercises.IsNullOrEmpty())
            {
                await AddExerciseAsync(arrayOfSnippets, exerciseName, description, language, splitted, username, dateCreated, dateUpdated);
                return Ok();
            }

            for(int i = 0; i < exercises[0].ArrayOfSnippets.Snippets.Count; i++)
            {
                Snippet currentSnippet = exercises[0].ArrayOfSnippets.Snippets[i];
                if (currentSnippet.ReadonlySection == false)
                {
                    currentSnippet.Code = arrayOfSnippets.snippets[i].Code;
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
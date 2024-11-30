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
    public async Task<IActionResult> UpdateDetails(string description, string exerciseName, string username, string newExerciseName, [FromBody] string[] tags)
    {
        List<Tag> allTags = _unitOfWork.Tags.CheckIfTagsExistElseCreate(tags);
        try
        {
            Teacher user = _unitOfWork.Teacher.GetByUsername(username);
            List<Exercise> exercises = await _unitOfWork.Exercises.GetExersiceByUsernameTeacherAsync(user, exerciseName);
            exercises = exercises
                .Where(exercise => exerciseName == exercise.Name)
                .ToList();

            exercises[0].Description = description;
            exercises[0].Tags = allTags;                 //TODO: jedes Tag schauen ob es schon in der Datenbank ist wenn nicht neu erstellen
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
        List<Tag> allTags = _unitOfWork.Tags.CreateTagsAndStoreInDB(tags);
        Teacher teacher = _unitOfWork.Teacher.GetByUsername(username);
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
                Description = description,
                Language = enumLanguage,
                Tags = allTags,                  //TODO: Tags neu erstellen
                TeacherId = teacher.Id,
                Teacher = teacher,
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
                    Code = snippet.code,
                    ReadonlySection = snippet.readonlySection,
                    FileName = snippet.fileName,
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
                return exercises
                    .Where(exercise => exercise.StudentId == null)
                    .Select(exercise => new ExerciseDto(
                exercise.Name,
                exercise.Teacher.Username,
                exercise.Description,
                ((Language)exercise.Language).ToString(),
                exercise.Tags.Select(tag => tag.Name).ToArray(),
                exercise.ArrayOfSnippets.Snippets.Select(snippet => new SnippetDto(
                        snippet.Code,
                        snippet.ReadonlySection,
                        snippet.FileName)).ToArray(),
                exercise.DateCreated,
                exercise.DateUpdated

                )).ToArray();
            }
            Teacher user = _unitOfWork.Teacher.GetByUsername(username);
            exercises = await _unitOfWork.Exercises.GetExersiceByUsernameTeacherAsync(user, exerciseName);
            return exercises
                .Where(exercise => exercise.StudentId == null)
                .Select(exercise => new ExerciseDto(
                    exercise.Name,
                    exercise.Teacher.Username,
                    exercise.Description,
                    ((Language)exercise.Language).ToString(),
                    exercise.Tags.Select(tag => tag.Name).ToArray(),
                    exercise.ArrayOfSnippets.Snippets.Select(snippet => new SnippetDto(
                        snippet.Code,
                        snippet.ReadonlySection,
                        snippet.FileName)).ToArray(),
                    exercise.DateCreated,
                    exercise.DateUpdated
                )).ToArray();
        }
        catch (Exception)
        {
            return [];
        }
    }

    [HttpGet("GetExerciseForStudentAssignment")]
    public async Task<ActionResult<ExerciseDto>> GetExerciseForStudentAssignment(string language, string exerciseName, string student)
    {
        ExerciseDto exercise = await _unitOfWork.Exercises.GetExerciseForStudentAssignment(language, exerciseName, student);
        if(exercise == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(exercise);
        }
    }


    [HttpPut]
    public async Task<IActionResult> UpdateExerciseForStudent(
    string student,
    string teacher,
    string description,
    string language,
    string subject,
    string exerciseName,
    DateTime dateCreated,
    DateTime dateUpdated,
    string[] tags,
    int total,
    int passed,
    int failed,
    [FromBody] ArrayOfSnippetsDto arrayOfSnippets)
    {
        try
        {
            // Extract tags and arrayOfSnippets from the body

            Student user = _unitOfWork.Student.GetByUsername(student);
            List<Exercise> exercises = await _unitOfWork.Exercises.GetExersiceByUsernameStudentAsync(user, exerciseName);
            exercises = exercises
                .Where(exercise => exerciseName == exercise.Name)
                .ToList();

            if (exercises.IsNullOrEmpty())
            {
                await AddExerciseForStudentAsync(arrayOfSnippets, exerciseName, description, language, tags, student, dateCreated, dateUpdated, teacher, total, passed, failed);

                return Ok();
            }
            exercises[0].TotalTests = total;
            exercises[0].PassedTests = passed;
            exercises[0].FailedTests = failed;
            for (int i = 0; i < exercises[0].ArrayOfSnippets.Snippets.Count; i++)
            {
                Snippet currentSnippet = exercises[0].ArrayOfSnippets.Snippets[i];
                if (!currentSnippet.ReadonlySection)
                {
                    currentSnippet.Code = arrayOfSnippets.snippets[i].code;
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

    private async Task<IActionResult> AddExerciseForStudentAsync(ArrayOfSnippetsDto arrayOfSnippets, string exerciseName, string description, string language, string[] tags, string studentUsername, DateTime dateCreated, DateTime dateUpdated, string teacherUsername, int total, int passed, int failed)
    {
        List<Tag> allTags = _unitOfWork.Tags.CreateTagsAndStoreInDB(tags);
        Teacher teacher = _unitOfWork.Teacher.GetByUsername(teacherUsername);
        Student student = _unitOfWork.Student.GetByUsername(studentUsername);
        Language enumLanguage = Language.CSharp;
        switch (language)
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
                Name = exerciseName,
                Description = description,
                Language = enumLanguage,
                Tags = allTags,                  //TODO: Tags neu erstellen
                TeacherId = teacher.Id,
                Teacher = teacher,
                Student = student,
                StudentId = student.Id,
                DateCreated = dateCreated,
                DateUpdated = dateUpdated,
                TotalTests = total,
                PassedTests = passed,
                FailedTests = failed
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
                    Code = snippet.code,
                    ReadonlySection = snippet.readonlySection,
                    FileName = snippet.fileName,
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

    public class UpdateExerciseRequestBodyDto
    {
        public string[] Tags { get; set; }
        public ArrayOfSnippetsDto ArrayOfSnippets { get; set; }
    }

}
using Core.Contracts;

namespace Persistence;

using Base.Persistence;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    public UnitOfWork() : this(new ApplicationDbContext())
    {

    }
    public UnitOfWork(ApplicationDbContext dBContext) : base(dBContext)
    {
        Students = new StudentRepository(dBContext);
        Teachers = new TeacherRepository(dBContext);
        ArrayOfSnippets = new ArrayOfSnippetsRepository(dBContext);
        Snippets = new SnippetRepository(dBContext);
        Exercises = new ExerciseRepository(dBContext);
        Assignments = new AssignmentsRepository(dBContext);
        Tags = new TagRepository(dBContext);
    }
    public ITagRepository Tags { get; }
    public IStudentRepository Students { get; }
    public ITeacherRepository Teachers { get; }

    public IExerciseRepository Exercises { get; }

    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }

    public ISnippetRepository Snippets { get; }
    public IAssignmentsRepository Assignments { get; set; }
}
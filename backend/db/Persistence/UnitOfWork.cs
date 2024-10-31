using Core.Contracts;

namespace Persistence;

using Base.Persistence;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    public UnitOfWork() : this(new ApplicationDbContext())
    {

    }
    public UnitOfWork(ApplicationDbContext dBContext) : base(dBContext)
    {
        Users = new UserRepository(dBContext);
        ArrayOfSnippets = new ArrayOfSnippetsRepository(dBContext);
        Snippets = new SnippetRepository(dBContext);
        Exercises = new ExerciseRepository(dBContext);
        Assignments = new AssignmentsRepository(dBContext);
        Tags = new TagRepository(dBContext);
    }
    public ITagRepository Tags { get; }
    public IUserRepository Users { get; }
    public IExerciseRepository Exercises { get; }

    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }

    public ISnippetRepository Snippets { get; }
    public IAssignmentsRepository Assignments { get; set; }
}
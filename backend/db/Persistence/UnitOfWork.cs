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
        ArrayOfSnippets = new ArrayOfSnippetsRepository(dBContext);
        Snippets = new SnippetRepository(dBContext);
        Exercises = new ExerciseRepository(dBContext);
        Assignments = new AssignmentsRepository(dBContext);
        Tags = new TagRepository(dBContext);
        AssignmentUser = new AssignmentUserRepository(dBContext);
        Teacher = new TeacherRepository(dBContext);
        Student = new StudentRepository(dBContext);
    }
    public ITagRepository Tags { get; }
    public IExerciseRepository Exercises { get; }

    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }

    public ISnippetRepository Snippets { get; }
    public IAssignmentsRepository Assignments { get; set; }
    public IAssignmentUserRepository AssignmentUser { get; set; }
    public ITeacherRepository Teacher { get; set; }
    public IStudentRepository Student { get; set; }
}
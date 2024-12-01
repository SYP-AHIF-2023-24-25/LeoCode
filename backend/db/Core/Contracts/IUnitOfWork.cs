namespace Core.Contracts;

using Base.Core.Contracts;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public IExerciseRepository Exercises { get; }
    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }
    public ISnippetRepository Snippets { get; }
    public IAssignmentsRepository Assignments { get; }
    public ITagRepository Tags { get; }
    public IAssignmentUserRepository AssignmentUser { get; }

    public ITeacherRepository Teacher { get; set; }

    public IStudentRepository Student { get; set; }
}
namespace Core.Contracts;

using Base.Core.Contracts;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public IStudentRepository Students { get; }
    public ITeacherRepository Teachers { get; }
    public IExerciseRepository Exercises { get; }
    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }
    public ISnippetRepository Snippets { get; }
    public IAssignmentsRepository Assignments { get; }
    public ITagRepository Tags { get; }

}
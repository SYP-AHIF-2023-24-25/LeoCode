namespace Core.Contracts;

using Base.Core.Contracts;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public IUserRepository Users { get; }
    public IExerciseRepository Exercises { get; }
    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }
    public ISnippetRepository Snippets { get; }
    public IAssignmentsRepository Assignments { get; }
    public ITagRepository Tags { get; }

}
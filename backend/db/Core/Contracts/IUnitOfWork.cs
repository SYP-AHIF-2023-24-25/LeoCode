namespace Core.Contracts;

using Base.Core.Contracts;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public IUserRepository Users { get; }
    public IExerciseRepository Exercises { get; }
    public ISnippetRepository Snippets { get; }
    public IArrayOfSnippets ArrayOfSnippets { get; }
}
﻿using Core.Contracts;

namespace Persistence;

using Base.Persistence;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

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
    }

    public IUserRepository Users  { get; }
    public IExerciseRepository Exercises { get; }

    public IArrayOfSnippetsRepository ArrayOfSnippets { get; }

    public ISnippetRepository Snippets { get; }
}
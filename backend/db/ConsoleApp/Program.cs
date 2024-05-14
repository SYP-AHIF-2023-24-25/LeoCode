

using Import;
using Persistence;

Console.WriteLine("Migrate Database");

    await using (var uow = new UnitOfWork(new ApplicationDbContext()))
    {
        await uow.DeleteDatabaseAsync();
        await uow.MigrateDatabaseAsync();

        var data = await ImportController.ImportDemoDataAsync();

        Console.WriteLine($"- {data.Exercises.Count} / 2 Exercises wurden erzeugt");
        Console.WriteLine($"- {data.Users.Count} / 2 Users wurden erzeugt");
        Console.WriteLine($"- {data.ArrayOfSnippets.Count} / 2 ArrayOfSnippets wurden erzeugt");
        Console.WriteLine($"- {data.Snippets.Count} / 6 Snippets wurden erzeugt");

        Console.WriteLine("Daten speichern");

        await uow.Users.AddRangeAsync(data.Users);
        await uow.Exercises.AddRangeAsync(data.Exercises);
        await uow.Snippets.AddRangeAsync(data.Snippets);
        await uow.ArrayOfSnippets.AddRangeAsync(data.ArrayOfSnippets);

        await uow.SaveChangesAsync();
    }


Console.WriteLine("done");
using Import;
using Persistence;

Console.WriteLine("Migrate Database");

await using (var uow = new UnitOfWork(new ApplicationDbContext()))
{
    await uow.DeleteDatabaseAsync();
    await uow.MigrateDatabaseAsync();

    var data = ImportController.ImportDemoData();

    Console.WriteLine($"- {data.Exercises.Count} Exercises wurden erzeugt");
    Console.WriteLine($"- {data.Users.Count} Users wurden erzeugt");
    Console.WriteLine($"- {data.ArrayOfSnippets.Count} ArrayOfSnippets wurden erzeugt");
    Console.WriteLine($"- {data.Snippets.Count} Snippets wurden erzeugt");

    Console.WriteLine(data.Users[0].Username);

    Console.WriteLine("Daten speichern");

    await uow.Users.AddRangeAsync(data.Users);
    await uow.Tags.AddRangeAsync(data.Tags);
    await uow.Exercises.AddRangeAsync(data.Exercises);
    await uow.ArrayOfSnippets.AddRangeAsync(data.ArrayOfSnippets);
    await uow.Snippets.AddRangeAsync(data.Snippets);
    await uow.Assignments.AddRangeAsync(data.Assignments);

    await uow.SaveChangesAsync();
}

Console.WriteLine("done");
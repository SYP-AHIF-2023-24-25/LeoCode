

using Persistence;

Console.WriteLine("Migrate Database");

await using (var uow = new UnitOfWork(new ApplicationDbContext()))
{
    await uow.DeleteDatabaseAsync();
    await uow.MigrateDatabaseAsync();

    await uow.SaveChangesAsync();
}

Console.WriteLine("done");
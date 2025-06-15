using Microsoft.EntityFrameworkCore;

namespace Database;

public static class DatabaseInitializer
{
    public static void InitDatabase()
    {
        using (var context = new DatabaseContext())
        {
            context.Database.Migrate();
        }
    }
}

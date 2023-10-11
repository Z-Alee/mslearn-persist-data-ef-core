namespace ContosoPizza.Data;
/// <summary>
/// Used for Program.cs to seed the database
/// CreateS an extension method for IHost that 
/// calls DbInitializer.Initialize:
/// </summary>
public static class Extensions
{
    // The CreateDbIfNotExists method is defined as an extension of IHost.
    public static void CreateDbIfNotExists(this IHost host)
    {
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // A reference to the PizzaContext service is created.
                var context = services.GetRequiredService<PizzaContext>();
                // EnsureCreated ensures that the database exists
                // If a database doesn't exist, EnsureCreated creates a new database
                context.Database.EnsureCreated();
                // The DbIntializer.Initialize method is called. 
                // The PizzaContext object is passed as a parameter.
                DbInitializer.Initialize(context);
            }
        }
    }
}
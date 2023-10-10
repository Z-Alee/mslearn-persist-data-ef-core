using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;

namespace ContosoPizza.Data;

public class PizzaContext : DbContext
{
    // Constructor of DbContetoptions<PizzaContext> allows
    // external code to pass in the configuration so that way
    // the same DbContext can be shared between test and prod code!!
    // and even be used with different providers 
    public PizzaContext (DbContextOptions<PizzaContext> options)
        : base(options)
        {

        }
    // These DbSet<T> properties correspond to tables to creat in the db
    // Table names will match the DbSet<T> proper names 
    // in the PizzaContext class. Can override behaviour as needed.
    // When instantiated, this PizzaContext class will expose these DbSets,
    // Pizzas, etc., properties. changes made to these collections will 
    // propgate to the database
    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<Topping> Toppings => Set<Topping>();
    public DbSet<Sauce> Sauces => Set<Sauce>();


}
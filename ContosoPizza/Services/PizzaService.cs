using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ContosoPizza.Services;

public class PizzaService
{
    // Class level field for PizzaContext before the constructor
    private readonly PizzaContext _context;
    /*
        The AddSplite method call added in the Program.cs registered PizzaContext
        for dependency injection. When the PizzaService is created,
        a PizzaContext is injected into the constructor   
        
        v This line
        builder.Services.AddSqlite<PizzaContext>("Data Source=ContosoPizza.db");
    */
    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    /*
        The Pizzas collection is ALL the rows in the Pizza Table
        AsNoTracking - tells EF Core to disable change tracking because
            this is a read-only operation
            This can optimize performance
        All Pizzas are returned with ToList
    */
    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas
            .AsNoTracking()
            .ToList();
    }

    /*
        AsNoTracking - read-only
        Include takes a lambda expression to indicator Toppings and Sauce
            Navigation Properties should be included in the result
            using Eagr Loading.
            Without this expression, EF Core returns NULL for those properties
                Eager loading: specify related data to be included in the query results
        SingleOrDefault - returns A Pizza that matches the lambda expression
            > if no records match, return null
            > if *mulitple* records match, return an exception
            > this lambda expression describes records where
                the Id property = id parameter 
    */
    public Pizza? GetById(int id)
    {
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }
    /*
        We assume at this point that the newPizza is valid.
            EF Core does not do any data validation. So either the
            runtime OR user code (or both) must validate it
        Add - adds the newPizza entity to the EF Core object graph
        SaveChanges - instructs EF Core to persist the object changes to the db 
        Return the newly saved/added Pizza
    */
    public Pizza? Create(Pizza newPizza)
    {
        _context.Add(newPizza);
        _context.SaveChanges();

        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        // Find existing pizzas and toppings by ID
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var toppingToAdd = _context.Toppings.Find(ToppingId);

        // if both do not exist, throw exception
        if (pizzaToUpdate == null || toppingToAdd is null) {
            throw new InvalidOperationException("Pizza or topping does not exist.");
        }

        // if only the Toppings is null/was not found, 
        // can create a new collection to add to the current pizza
        if (pizzaToUpdate.Toppings is null) {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        // Add the topping to the existing or new collection
        pizzaToUpdate.Toppings.Add(toppingToAdd);
        // instructs EF Core to persist object changes to the db
        _context.SaveChanges();
    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        // Find existing pizzas and toppings by ID
        /*
            ind is an optimized method to query records by their 
            primary key. 
            Find searches the local entity graph first before 
            it queries the database.
        */
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var sauceToUpdate = _context.Sauces.Find(SauceId);

         // if both do not exist, throw exception
        if (pizzaToUpdate == null || sauceToUpdate is null) {
            throw new InvalidOperationException("Pizza or sauce does not exist.");
        }
        // An Update method call is unnecessary because 
        // EF Core detects that you set the Sauce property on Pizza.
        pizzaToUpdate.Sauce = sauceToUpdate;
        // instructs EF Core to persist object changes to the db
        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = _context.Pizzas.Find(id);
        if (pizzaToDelete is not null)
        {
            // The Remove method removes the 
            // pizzaToDelete entity in EF Core's object graph.
            _context.Pizzas.Remove(pizzaToDelete);
            _context.SaveChanges();
        }     
    }
}
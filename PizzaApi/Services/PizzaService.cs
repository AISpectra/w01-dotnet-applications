using PizzaApi.Models;

namespace PizzaApi.Services;

public static class PizzaService
{
    private static readonly List<Pizza> Pizzas =
    [
        new() { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
        new() { Id = 2, Name = "Veggie", IsGlutenFree = true },
        new() { Id = 3, Name = "Pepperoni Feast", IsGlutenFree = false }
    ];

    private static int nextId = 4;

    public static List<Pizza> GetAll() => Pizzas;

    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(pizza => pizza.Id == id);

    public static void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }

    public static void Delete(int id)
    {
        Pizza? pizza = Get(id);

        if (pizza is null)
        {
            return;
        }

        Pizzas.Remove(pizza);
    }

    public static void Update(Pizza pizza)
    {
        int index = Pizzas.FindIndex(existingPizza => existingPizza.Id == pizza.Id);

        if (index == -1)
        {
            return;
        }

        Pizzas[index] = pizza;
    }
}

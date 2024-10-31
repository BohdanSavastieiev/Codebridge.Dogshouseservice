using Codebridge.TechnicalTask.Domain.Dogs.Entities;

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Seed;

public static class DogSeedData
{
    public static readonly IReadOnlyList<Dog> InitialDogs = new List<Dog>
    {
        new("Neo", "red & amber", 22, 32),
        new("Jessy", "black & white", 7, 14),
        new("Max", "brown", 15, 25),
        new("Luna", "black", 0, 20),
        new("Rocky", "white & brown", 18, 30)
    };
}
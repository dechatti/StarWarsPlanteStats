public readonly record struct Planet
{
    public string Name { get; }
    public int? Diameter { get; }
    public int? Surfacewater { get; }
    public long? Population { get; }
    public Planet(string name,
        int? diameter,
        int? surfacewater,
        long? population
        ) 
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        Name = name;
        Diameter = diameter;
        Surfacewater = surfacewater;
        Population = population;
    }

    public static explicit operator Planet(Root planetDto)
    {
        var name = planetDto.name;
        int? diameter = planetDto.diameter.ToIntOrNull();
        long? population = planetDto.population.ToLongOrNull();
        int? surfacewater = planetDto.surface_water.ToIntOrNull();

        return new Planet(name, diameter, surfacewater, population);
    }

    
}


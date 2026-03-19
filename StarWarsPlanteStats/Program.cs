
using StarWarsPlanteStats.DataAccess;
using System.Text.Json;

try
{
    await new StarWarsPlanetStatsApp(
        new apiDataReader(),
        new MockStarWarsApiDataReader()).Run();
}
catch(Exception ex)
{
    Console.WriteLine("An unexpected error has occured" +
        "Exception message" + ex.Message);
}

Console.WriteLine("Press any key to close.");
Console.ReadKey();
public class StarWarsPlanetStatsApp
{
    private readonly IapiDataReader _apiDataReader;
    private readonly IapiDataReader _secondaryApiDataReader;
    public StarWarsPlanetStatsApp(
        IapiDataReader apiDataReader,
        IapiDataReader secondaryApiDataReader)
    {
        _apiDataReader = apiDataReader;
        _secondaryApiDataReader = secondaryApiDataReader;
    }

    
    public async Task Run()
    {
        string? json = null;
        try
        {
            json = await _apiDataReader.Read("https://swapi.info/api/", "planets");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("API Request was unsuccessful. " +
                "Switching to Mock data." +
                "Exception Message: " +
                ex.Message);
        }
        if (json is null)
        {
            json = await _secondaryApiDataReader.Read("https://swapi.info/api/", "planets");
        }
        var root = JsonSerializer.Deserialize<List<Root>>(json);
        var planets = ToPlanets(root);
        foreach (var planet in planets)
        {
            Console.WriteLine(planet);
        }
        Console.WriteLine();
        Console.WriteLine("The statistics of which property would you like to see ?");
        Console.WriteLine("population");
        Console.WriteLine("diameter");
        Console.WriteLine("surface water");
        var userchoice = Console.ReadLine();

        if (userchoice == "population")
        {
            var planetwithmaxpopulation = planets.MaxBy(planet => planet.Population);
            Console.WriteLine($"max population is: " +
                $"{planetwithmaxpopulation.Population}" +
                $"(Planet: {planetwithmaxpopulation.Name})");

            var planetwithminpopulation = planets.MinBy(planet => planet.Population);
            Console.WriteLine($"Min population is: " +
                $"{planetwithminpopulation.Population}" +
                $"(Planet: {planetwithminpopulation.Name})");
        }
        else if (userchoice == "diameter")
        {
            var planetwithmaxdiameter = planets.MaxBy(planet => planet.Diameter);
            Console.WriteLine($"max diameter is: " +
                $"{planetwithmaxdiameter.Diameter}" +
                $"(Planet: {planetwithmaxdiameter.Name})");

            var planetwithmindiameter = planets.MinBy(planet => planet.Diameter);
            Console.WriteLine($"Min diameter is: " +
                $"{planetwithmindiameter.Diameter}" +
                $"(Planet: {planetwithmindiameter.Name})");
        }
        else if (userchoice == "surface water")
        {
            var planetwithmaxsurfacewater = planets.MaxBy(planet => planet.Surfacewater);
            Console.WriteLine($"max surface water is: " +
                $"{planetwithmaxsurfacewater.Surfacewater}" +
                $"(Planet: {planetwithmaxsurfacewater.Name})");

            var planetwithminsurfacewater = planets.MinBy(planet => planet.Surfacewater);
            Console.WriteLine($"Min surface water is: " +
                $"{planetwithminsurfacewater.Surfacewater}" +
                $"(Planet: {planetwithminsurfacewater.Name})");
        }
        else
        {
            Console.WriteLine("Invalid Choice.");
        }

    }
    private void ShowStatistics(IEnumerable<Planet> planets,
        string propertyname, Func<Planet, int> propertyselector)
    {
        var planetwithmaxpropertyvalue = planets.MaxBy(propertyselector);
        Console.WriteLine($"max {propertyname} is: " +
            $"{propertyselector(planetwithmaxpropertyvalue)}" +
            $"(Planet: {planetwithmaxpropertyvalue.Name})");

        var planetwithminpropertyvalue = planets.MinBy(propertyselector);
        Console.WriteLine($"Min {propertyname} is: " +
            $"{propertyselector(planetwithminpropertyvalue)}" +
            $"(Planet: {planetwithminpropertyvalue.Name})");
    }

    private IEnumerable<Planet> ToPlanets(List<Root>? root)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        var planets = new List<Planet>();
        foreach (var planetDto in root)
        {
            Planet planet1 = (Planet)planetDto;
                planets.Add(planet1);
        }
        return planets;
    }
}
public readonly record struct Planet
{
    public string Name { get; }
    public int? Diameter { get; }
    public int? Surfacewater { get; }
    public int? Population { get; }
    public Planet(string name,
        int? diameter,
        int? surfacewater,
        int? population) 
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
        int? population = planetDto.population.ToIntOrNull();
        int? surfacewater = planetDto.surface_water.ToIntOrNull();

        return new Planet(name, diameter, population, surfacewater);
    }

    
}
public static class StringsExtension
{
    public static int? ToIntOrNull(this string? input)
    {
        int? result = null;
        if (int.TryParse(input, out int resultparsed))
        {
            result = resultparsed;
        }
        return result;
    }
}


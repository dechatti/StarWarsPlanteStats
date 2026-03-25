using System.Text.Json;

public class PlanetsFromApiReader : IPlanetsReader
{
    private readonly IapiDataReader _apiDataReader;
    private readonly IapiDataReader _secondaryApiDataReader;
    private readonly IUserInteractor _userInteractor;
    public PlanetsFromApiReader(IapiDataReader apiDataReader, IapiDataReader secondaryApiDataReader, IUserInteractor userinteractor)
    {
        _apiDataReader = apiDataReader;
        _secondaryApiDataReader = secondaryApiDataReader;
        _userInteractor = userinteractor;
    }
    public async Task<IEnumerable<Planet>> Read()
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
        json ??= await _secondaryApiDataReader.Read("https://swapi.info/api/", "planets");
        var root = JsonSerializer.Deserialize<List<Root>>(json);
        return ToPlanets(root);
    }
    private static IEnumerable<Planet> ToPlanets(List<Root>? root)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        return root.Select(planetDto => (Planet)planetDto);

    }
}


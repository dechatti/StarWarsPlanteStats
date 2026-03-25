public class PlanetsStatisticsAnalyzer : IPlanetsStatisticsAnalyzer
{
    private readonly IPlanetsStatsUserInteractor _planetsStatsUserInteractor;
    public PlanetsStatisticsAnalyzer(
        IPlanetsStatsUserInteractor planetsStatsUserInteractor)
    {
        _planetsStatsUserInteractor = planetsStatsUserInteractor;
    }
    private readonly Dictionary<string, Func<Planet, long?>> _propertynamesToSelectorsMapping =
        new ()
        {
            ["population"] = planet => planet.Population,
            ["diameter"] = planet => planet.Diameter,
            ["Surface water"] = planet => planet.Surfacewater,
        };
    public void Analyze(IEnumerable<Planet> planets)
    {
        var userchoice = _planetsStatsUserInteractor.ChooseStatisticsToBeShown(_propertynamesToSelectorsMapping.Keys);

        if (userchoice is null || !_propertynamesToSelectorsMapping.ContainsKey(userchoice))
        {
            _planetsStatsUserInteractor.ShowMessage("Invalid Choice.");
        }
        else
        {
            ShowStatistics(planets,
                userchoice,
                _propertynamesToSelectorsMapping[userchoice]);
        }
    }
        private static void ShowStatistics(IEnumerable<Planet> planets,
        string propertyname, Func<Planet, long?> propertyselector)
    {
        ShowStatistics(
            "Max",
            planets.MaxBy(propertyselector),
            propertyselector, propertyname);

        ShowStatistics(
            "Min",
            planets.MinBy(propertyselector),
            propertyselector, propertyname);
    }

    private static void ShowStatistics(string descriptor,
        Planet selectedPlanet, Func<Planet, long?> propertyselector, string propertyname)
    {
        Console.WriteLine($"{descriptor} {propertyname} is: " +
            $"{propertyselector(selectedPlanet)}" +
            $"(Planet: {selectedPlanet.Name})");
    }
}


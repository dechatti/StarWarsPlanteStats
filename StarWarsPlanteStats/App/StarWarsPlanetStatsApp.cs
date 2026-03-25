public class StarWarsPlanetStatsApp
{
    private readonly IPlanetsReader _planetsReader;
    private readonly IPlanetsStatisticsAnalyzer _planetsStatisticsAnalyzer;
    private readonly IPlanetsStatsUserInteractor _planetsStatsUserInteractor;
    public StarWarsPlanetStatsApp(
        IPlanetsReader planetsReader,
        IPlanetsStatisticsAnalyzer planetsStatisticsAnalyzer,
        IPlanetsStatsUserInteractor planetsstatsuserinteractor)
    {
        _planetsReader = planetsReader;
        _planetsStatisticsAnalyzer = planetsStatisticsAnalyzer;
        _planetsStatsUserInteractor = planetsstatsuserinteractor;
    }


    public async Task Run()
    {
        var planets = await _planetsReader.Read();
        _planetsStatsUserInteractor.Show(planets);

        _planetsStatisticsAnalyzer.Analyze(planets);

    }
}


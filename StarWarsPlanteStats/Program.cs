
using StarWarsPlanteStats.DataAccess;
using System.Numerics;

try
{
    var consoleuserinteractor = new ConsoleUserInteractor();
    var planetsstatsuserinteractor = new PlanetsStatsUserInteractor(consoleuserinteractor);

    await new StarWarsPlanetStatsApp(
        new PlanetsFromApiReader(
        new apiDataReader(),
        new MockStarWarsApiDataReader(), consoleuserinteractor),
        new PlanetsStatisticsAnalyzer(planetsstatsuserinteractor), planetsstatsuserinteractor).Run();
}
catch(Exception ex)
{
    Console.WriteLine("An unexpected error has occurred. " +
        "Exception message:  " + ex.Message);
}

Console.WriteLine("Press any key to close.");
Console.ReadKey();


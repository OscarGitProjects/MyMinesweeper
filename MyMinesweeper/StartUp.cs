using Microsoft.Extensions.DependencyInjection;
using MyMinesweeper.Interface;
using MyMinesweeper.UI;
using MyMinesweeper.Utils;

namespace MyMinesweeper;

public class StartUp
{
    public void SetUp()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        ConsoleMineSweeper? mineSweeper = serviceProvider.GetService<ConsoleMineSweeper>();
        mineSweeper?.Run("Minesweeper", new Board(10, 10));
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<ConsoleMineSweeper>();
        services.AddSingleton<IUI, ConsoleUI>();
        services.AddSingleton<ISweeper, Sweeper>();
        services.AddSingleton<IBoardHelper, BoardHelper>();
    }
}

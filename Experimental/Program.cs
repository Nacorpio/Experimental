using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Experimental.Common.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Experimental
{

  class Program
  {
    public const string ClientToken = "NzQ4NTYzMDc0MDAwMjI0NDQ3.X0fPsA.5S2AbGl0-B7QpeU9ukUJWIhU6IE";

    private static void Main(string[] args)
    {
      new Program().Run().GetAwaiter().GetResult();
    }

    public async Task Run()
    {
      var services = ConfigureServices();

      await services
       .GetRequiredService<CommandHandlingService>()
       .InitializeAsync();

      await Global.PrepareStoresAsync();

      await Global.Client.LoginAsync(TokenType.Bot, ClientToken);
      await Global.Client.StartAsync();

      await Task.Delay(-1);
    }

    private static IServiceProvider ConfigureServices()
    {
      return new ServiceCollection()
          .AddSingleton(Global.Client)
          .AddSingleton<CommandService>()
          .AddSingleton<CommandHandlingService>()
          .BuildServiceProvider();
    }
  }
}

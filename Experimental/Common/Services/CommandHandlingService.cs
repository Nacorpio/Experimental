using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using JetBrains.Annotations;

namespace Experimental.Common.Services
{

  public class CommandHandlingService : HandlingService
  {
    public const string Prefix = "+";
    private readonly CommandService _commandService;

    public CommandHandlingService([NotNull] IServiceProvider provider, [NotNull] DiscordSocketClient client,
        [NotNull] CommandService commandService) : base(provider, client)
    {
      _commandService = commandService;
      Client.MessageReceived += OnMessageReceived;
    }

    public async Task InitializeAsync()
    {
      // Register modules that are public and inherit ModuleBase<T>.
      await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), Provider);
    }

    private async Task OnMessageReceived(SocketMessage arg)
    {
      if (!(arg is SocketUserMessage msg)) return;
      if (msg.Source != MessageSource.User) return;

      var argPos = 0;

      if (!(msg.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
            msg.HasStringPrefix(Prefix, ref argPos))) return;

      var context = new SocketCommandContext(Client, msg);
      var result = await _commandService.ExecuteAsync(context, argPos, Provider);

      if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
      {
        var errorMsg = await context.Channel.SendMessageAsync(result.ToString());

        await Task
            .Delay(TimeSpan.FromSeconds(3))
            .ContinueWith(x => errorMsg.DeleteAsync());

        return;
      }

      await msg.DeleteAsync();
    }
  }

}
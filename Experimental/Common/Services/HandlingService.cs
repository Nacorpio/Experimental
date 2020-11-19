using System;

using Discord.WebSocket;

using JetBrains.Annotations;

namespace Experimental.Common.Services
{

  public abstract class HandlingService
  {
    public DiscordSocketClient Client { get; }
    public IServiceProvider Provider { get; }

    protected HandlingService([NotNull] IServiceProvider provider, [NotNull] DiscordSocketClient client)
    {
      Provider = provider;
      Client = client;
    }
  }

}
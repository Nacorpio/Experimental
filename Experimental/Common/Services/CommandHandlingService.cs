using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Commands.Builders;
using Discord.WebSocket;

using Experimental.Common.Builders;
using Experimental.Common.Extensions;
using Experimental.Common.Modules;
using Experimental.Common.Objects;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet;
using UnitsNet.Units;

namespace Experimental.Common.Services
{

  public static class RandomExtensions
  {
    public static Random Instance = new Random();

    public static T Random<T>(this IEnumerable<T> _)
    {
      return _.ElementAt(Instance.Next(0, _.Count() - 1));
    }
  }

  public class CommandHandlingService : HandlingService
  {
    private Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo> _substanceDoseLevelMap;

    public const string Prefix = "+";
    private readonly CommandService _commandService;

    public CommandHandlingService([NotNull] IServiceProvider provider, [NotNull] DiscordSocketClient client,
        [NotNull] CommandService commandService) : base(provider, client)
    {
      _commandService = commandService;
      _substanceDoseLevelMap = new Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo>();

      Client.MessageReceived += OnMessageReceived;
    }

    public async Task InitializeAsync()
    {
      // Register modules that are public and inherit ModuleBase<T>.
      await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), Provider);
      await CreateCommandsAsync();
    }

    private static readonly Dictionary<SubstanceRouteType, string> _substanceRouteNameMap = new Dictionary<SubstanceRouteType, string>
    {
      {SubstanceRouteType.Oral, "take"},
      {SubstanceRouteType.Nasal, "snort"},
      {SubstanceRouteType.Rectal, "boof"},
      {SubstanceRouteType.Smoked, "smoke"},
      {SubstanceRouteType.Vaporized, "vape"},
      {SubstanceRouteType.Intravenous, "inject"}
    };

    private static readonly Dictionary<SubstanceRouteType, string[]> _substanceRouteAliasMap = new Dictionary<SubstanceRouteType, string[]>
    {
      {SubstanceRouteType.Smoked, new []{"rök", "puff", "puffa", "inhalera", "inhale"}},
      {SubstanceRouteType.Nasal, new []{"snorta", "dra", "line"}},
      {SubstanceRouteType.Vaporized, new []{"vapea"}},
      {SubstanceRouteType.Intravenous, new []{"skjut", "injicera", "shoot"}},
      {SubstanceRouteType.Rectal, new []{"boofa", "plug", "rp", "rövplugga", "rövplugg", "rövplug", "rövpanna"}},
      {SubstanceRouteType.Oral, new []{"ta", "svälj", "käka", "ät", "drick", "drink", "eat", "consume", "swallow"}}
    };

    public static readonly Dictionary<SubstanceRouteType, string[]> _substanceRouteActionPhraseMap = new Dictionary<SubstanceRouteType, string[]>
    {
      { SubstanceRouteType.Oral, new [] {"took", "ate", "swallowed"}},
      { SubstanceRouteType.Smoked, new []{"smoked", "puffed"}},
      { SubstanceRouteType.Intramuscular, new []{"shot", "injected"}},
      { SubstanceRouteType.Vaporized, new []{"vaped", "puffed"}},
      { SubstanceRouteType.Nasal, new []{"snorted", "vacuumed"}}
    };

    private async Task CreateCommandsAsync()
    {
      await _commandService.CreateModuleAsync(
        string.Empty,
        x =>
        {
          x
           .WithName("Administration")
           .WithSummary("Commands that regard consumption of substances.");

          foreach (var entryData in Global.Store.GetObjects<SubstanceEntryData>())
          {
            var route = entryData.Route;

            x.AddCommand(_substanceRouteNameMap[route], OnConsumeSubstance, _ =>
              {
                var name = Enum
                 .GetNames(typeof(SubstanceRouteType))[(int)route]
                 .ToLower(GlobalModule.Swedish);

                _.Name = _substanceRouteNameMap[route];
                _.Summary = $"Consumes a substance by the {name} route of administration at the given dosage .";
                _.IgnoreExtraArgs = true;

                _.AddAliases(_substanceRouteAliasMap[route]);

                _.AddParameter(
                  "substance_name",
                  typeof(string),
                  parameter => parameter
                   .WithSummary("The name of the substance to consume.")
                   .WithIsRemainder(false)
                   .WithIsOptional(false)
                );

                _.AddParameter(
                  "substance_dose",
                  typeof(string),
                  parameter => parameter
                   .WithSummary("The dose of the substance to consume.")
                   .WithIsRemainder(true)
                   .WithIsOptional(true)
                );
              }
            );
          }
        }
      );
    }

    private static async Task OnConsumeSubstance(ICommandContext context, object[] arg2, IServiceProvider arg3, CommandInfo arg4)
    {
      var substance_name = arg2[0] as string;
      var substance_dose = arg2[1] as string;

      if (substance_name == null)
      {
        return;
      }
      var substance = Global.GetSubstance(substance_name);

      if (substance == null)
      {
        await context.Channel.SendMessageAsync(
          null,
          false,
          new EmbedBuilder()
           .WithThumbnailUrl(Emotes.EmoteProhibited)
           .WithTitle("Couldn't find a substance with the specified name.")
           .WithDescription("Make sure that you spelled the name correctly!")
           .WithGeneralInfo((IGuildUser)context.User)
           .Build());

        return;
      }

      var dose = Global
       .GetStore(context.Guild)
       .CreateObject<SubstanceUserDoseInfo, SubstanceUserDoseBuilder>(
          x => x
           .WithSubstance(substance)
           .WithUser(context.User)
           .WithTakenAt(DateTimeOffset.Now)
           .WithRoute(_substanceRouteNameMap.FirstOrDefault(y => y.Value.Equals(arg4.Name)).Key)
        );

      if (substance_dose == null)
      {
        await context.Channel.SendMessageAsync(
        null, false,
        new EmbedBuilder()
         .WithThumbnailUrl(substance.ImageUrl)
         .WithTitle($"A user just {_substanceRouteActionPhraseMap[dose.Route].Random()}  **{substance.DisplayName.ToLower(GlobalModule.Swedish)}**")
         .AddField("Message", $"{context.User.Mention} was taking the substance using the **{GlobalModule.RouteWordMap[dose.Route]}**"
                   + $" route of administration.\n\n"
                   + $"🔒 The data has been logged securely.")
         .WithGeneralInfo((IGuildUser)context.User)
         .Build());

        return;
      }

      if (!Mass.TryParse(substance_dose, GlobalModule.Swedish, out var amount))
      {
        return;
      }

      dose.Amount = amount;

      await context.Channel.SendMessageAsync(
        null, false,
        new EmbedBuilder()
         .WithThumbnailUrl(substance.ImageUrl)
         .WithTitle($"A user just {_substanceRouteActionPhraseMap[dose.Route].Random()} **{amount.ToString(GlobalModule.Swedish)}** of **{substance.DisplayName.ToLower(GlobalModule.Swedish)}**")
         .AddField("Message", $"{context.User.Mention} was taking the substance using the **{GlobalModule.RouteWordMap[dose.Route]}**"
                   + $" route of administration and the amount of substance was"
                   + $" **{amount.ToString(GlobalModule.Swedish)}**.\n\n"
                   + $"🔒 The data has been logged securely.")
         .WithGeneralInfo((IGuildUser)context.User)
         .Build());
    }

    private CommandBuilder ConsumeSubstance
    (
      [NotNull] CommandBuilder builder,
      [NotNull] string substanceName,
      [NotNull] string summary,
      [NotNull] Mass amount,
      [NotNull] SubstanceRouteType route
    )
    {
      throw new NotImplementedException();
    }

    private Task OnSnortCommand(ICommandContext arg1, object[] arg2, IServiceProvider arg3, CommandInfo arg4)
    {
      throw new NotImplementedException();
    }

    private Task OnEatCommand(ICommandContext arg1, object[] arg2, IServiceProvider arg3, CommandInfo arg4)
    {
      throw new NotImplementedException();
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
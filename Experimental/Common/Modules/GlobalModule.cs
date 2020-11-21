using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Experimental.Common.Objects.Data;

using UnitsNet;
using UnitsNet.Units;

namespace Experimental.Common.Modules
{

  public class GlobalModule : ModuleBase<SocketCommandContext>
  {
    internal static readonly Dictionary<SubstanceRouteType, string> RouteWordMap =
      new Dictionary<SubstanceRouteType, string>
      {
        { SubstanceRouteType.Oral, "oral" },
        { SubstanceRouteType.Nasal, "nasal" },
        { SubstanceRouteType.Sublingual, "sublingual" },
        { SubstanceRouteType.Smoked, "smoked" },
        { SubstanceRouteType.Intravenous, "intravenous" },
        { SubstanceRouteType.Intramuscular, "intramuscular" },
        { SubstanceRouteType.Rectal, "rectal" }
      };

    public static CultureInfo Swedish => new CultureInfo("sv-SE");

    [Command("mdmadose", true)]
    public async Task MdmaDose(string bodyWeight)
    {
      if (!Mass.TryParse(bodyWeight, Swedish, out var weight))
      {
        return;
      }

      var safeDose = new Mass(weight.Kilograms * 1.5d, MassUnit.Milligram);

      await Context.Channel
       .SendMessageAsync(
          null, false, new EmbedBuilder()
           .WithAuthor("MDMA dose calculator", Emotes.EmoteRainbow)
           .WithDescription("This dose has been calculated according to the formula given by rollsafe.org.")
           .WithColor(Color.Purple)
           .AddField(
                    name: "⭐ Recommended safe dose",
                    value: $"The calculated safe dose for you according to rollsafe.org is **{safeDose.ToString(Swedish)}**.")
           .AddField(
                    name: "💧 Water",
                    value: $"When taking MDMA in any of its forms it is important to hydrate moderately through out the roll. "
                         + $"Make sure to drink plenty of water, but **don't exceed 0,5L** per hour.")
           .WithCurrentTimestamp()
           .Build());
    }
  }

}
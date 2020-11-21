using System;
using System.Collections.Generic;
using System.Linq;

using Discord;

using Experimental.Common;
using Experimental.Common.Objects;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet.Units;

namespace Experimental
{

  public static partial class Global
  {
    private static void CreateSubstances()
    {
      Store.GetManager<Substance>()
       .AddRange(new ManagedObject[]
        {
          Substance.CreateInstance(
            x => x
             .WithName("amphetamine")
             .WithDisplayName("Amphetamine")
             .WithCategory(SubstanceCategory.Stimulant)
             .WithBaseUnit(MassUnit.Milligram)
             .WithColor(Color.LighterGrey)
             .WithImageUrl(Emotes.EmoteDashingAway)
             .AddAliases(new[]{"tjack", "speed", "dos"})
             .AddEntry(
                y => y
                  .WithRoute(SubstanceRouteType.Nasal)
                  .AddLevel(SubstanceDoseLevelType.Light, 6, 15, MassUnit.Milligram)
                  .AddLevel(SubstanceDoseLevelType.Common, 15, 30, MassUnit.Milligram)
                  .AddLevel(SubstanceDoseLevelType.Strong, 30, 50, MassUnit.Milligram)
                  .AddLevel(SubstanceDoseLevelType.Heavy, 50, 100, MassUnit.Milligram)
                  .AddDuration(SubstanceDurationType.Onset, 1, 5, DurationUnit.Minute)
                  .AddDuration(SubstanceDurationType.Peak, 1, 2, DurationUnit.Hour)
                  .AddDuration(SubstanceDurationType.Offset, 1.5d, 3, DurationUnit.Hour)
                  .AddDuration(SubstanceDurationType.AfterEffects, 2, 4, DurationUnit.Hour)
              )
             .AddEntry(
                y => y
                 .WithRoute(SubstanceRouteType.Oral)
                 .AddLevel(SubstanceDoseLevelType.Light, 7.5d, 20, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Common, 20, 40, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Strong, 40, 70, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Heavy, 70, 120, MassUnit.Milligram)
                 .AddDuration(SubstanceDurationType.Onset, 15, 30, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Peak, 2.5d, 4, DurationUnit.Hour)
                 .AddDuration(SubstanceDurationType.Offset, 2, 3, DurationUnit.Hour)
                 .AddDuration(SubstanceDurationType.AfterEffects, 3, 6, DurationUnit.Hour)
              )
          ),
          Substance.CreateInstance(
            x => x
             .WithName("weed")
             .WithDisplayName("Weed")
             .WithCategory(SubstanceCategory.Cannabinoid)
             .WithBaseUnit(MassUnit.Gram)
             .WithColor(Color.Green)
             .WithImageUrl(Emotes.EmoteHerb)
             .AddAliases(new[]{"gräs", "bud", "kush", "haze", "grass", "joint"})
             .AddEntry(
                y => y
                 .WithRoute(SubstanceRouteType.Smoked)
                 .AddLevel(SubstanceDoseLevelType.Light, 50, 100, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Common, 100, 150, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Strong, 150, 250, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Heavy, 250, 500, MassUnit.Milligram)
                 .AddDuration(SubstanceDurationType.Onset, 0, 10, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Peak, 15, 30, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.AfterEffects, 45, 180, DurationUnit.Minute)
              )
          ),
          Substance.CreateInstance(
            x => x
             .WithName("hash")
             .WithDisplayName("Hash")
             .WithCategory(SubstanceCategory.Cannabinoid)
             .WithBaseUnit(MassUnit.Gram)
             .WithColor(Color.DarkGreen)
             .WithImageUrl(Emotes.EmoteHerb)
             .AddAliases(new []{"bruning", "brunt", "brun", "hashish", "hasch"})
             .AddEntry(
                y => y
                 .WithRoute(SubstanceRouteType.Smoked)
                 .AddLevel(SubstanceDoseLevelType.Light, 50, 100, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Common, 100, 150, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Strong, 150, 250, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Heavy, 250, 500, MassUnit.Milligram)
                 .AddDuration(SubstanceDurationType.Onset, 0, 10, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Peak, 15, 30, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.AfterEffects, 45, 180, DurationUnit.Minute)
              )
          ),
          Substance.CreateInstance(
            x => x
             .WithName("mdma")
             .WithDisplayName("MDMA")
             .WithCategory(SubstanceCategory.Stimulant)
             .WithBaseUnit(MassUnit.Milligram)
             .WithColor(Color.Purple)
             .WithImageUrl(Emotes.EmoteHeart)
             .AddAliases(new []{"molly", "m", "mandy"})
             .AddEntry(
                y => y
                 .WithRoute(SubstanceRouteType.Oral)
                 .AddLevel(SubstanceDoseLevelType.Light, 45, 75, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Common, 75, 140, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Strong, 140, 180, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Heavy, 180, 250, MassUnit.Milligram)
                 .AddDuration(SubstanceDurationType.Onset, 30, 60, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Comeup, 15, 30, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Peak, 1.5, 2.5, DurationUnit.Hour)
                 .AddDuration(SubstanceDurationType.Offset, 1, 1.5, DurationUnit.Hour)
                 .AddDuration(SubstanceDurationType.AfterEffects, 12, 48, DurationUnit.Hour)
              )
          ),
          Substance.CreateInstance(
            x => x
             .WithName("ecstasy")
             .WithDisplayName("Ecstasy")
             .WithCategory(SubstanceCategory.Stimulant)
             .WithBaseUnit(MassUnit.Milligram)
             .WithColor(Color.Purple)
             .WithImageUrl(Emotes.EmoteHeart)
             .AddAliases(new[]{"xtc", "e", "eva", "knapp", "knappar"})
             .AddEntry(
                y => y
                 .WithRoute(SubstanceRouteType.Oral)
                 .AddLevel(SubstanceDoseLevelType.Light, 45, 75, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Common, 75, 140, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Strong, 140, 180, MassUnit.Milligram)
                 .AddLevel(SubstanceDoseLevelType.Heavy, 180, 250, MassUnit.Milligram)
                 .AddDuration(SubstanceDurationType.Onset, 30, 60, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Comeup, 15, 30, DurationUnit.Minute)
                 .AddDuration(SubstanceDurationType.Peak, 1.5, 2.5, DurationUnit.Hour)
                 .AddDuration(SubstanceDurationType.Offset, 1, 1.5, DurationUnit.Hour)
                 .AddDuration(SubstanceDurationType.AfterEffects, 12, 48, DurationUnit.Hour))
          )
        });
    }

    /// <summary>
    /// Finds and returns a substance with the specified identifier.
    /// </summary>
    /// <param name="substanceGuid">The GUID of the substance to find.</param>
    /// <returns>A reference to the substance, if found; otherwise, <c>null</c>.</returns>
    public static Substance GetSubstance(Guid substanceGuid)
    {
      return Store.GetObject<Substance>(substanceGuid);
    }

    /// <summary>
    /// Finds and returns a substance matching the specified name.
    /// </summary>
    /// <param name="substanceName">The name of the substance to find.</param>
    /// <returns>A reference to the substance, if found; otherwise, <c>null</c>.</returns>
    public static Substance GetSubstance([NotNull] string substanceName)
    {
      return Store
       .GetObjects<Substance>()
       .FirstOrDefault(x =>
         x.Name.Equals(substanceName, StringComparison.OrdinalIgnoreCase) ||
         x.DisplayName.Equals(substanceName, StringComparison.InvariantCultureIgnoreCase) ||
         x.Aliases.Any(alias => string.Equals(alias, substanceName)));
    }


    /// <summary>
    /// Finds and returns a collection of substances belonging to the specified category.
    /// </summary>
    /// <param name="category">The category to find substances of.</param>
    /// <returns>A reference to the stored collection of substances.</returns>
    public static IEnumerable<Substance> GetSubstances([NotNull] SubstanceCategory category)
    {
      return Store.GetObjects<Substance>()
       .Where(x => x.Category == category);
    }

    /// <summary>
    /// Returns a collection of all registered substances.
    /// </summary>
    /// <returns>A reference to the stored collection of substances.</returns>
    public static IEnumerable<Substance> GetSubstances()
    {
      return Store.GetObjects<Substance>();
    }
  }

}
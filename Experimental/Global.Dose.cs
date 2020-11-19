using System;
using System.Collections.Generic;
using System.Linq;

using Discord;

using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

namespace Experimental
{

  public static partial class Global
  {
    /// <summary>
    /// Returns the doses that the specified user has taken.
    /// </summary>
    /// <param name="guild">The guild to get the doses of.</param>
    /// <returns>A reference to the new collection of doses.</returns>
    public static IEnumerable<SubstanceUserDoseInfo> GetDoses([NotNull] IGuild guild)
    {
      return GetStore(guild).GetObjects<SubstanceUserDoseInfo>();
    }

    /// <summary>
    /// Filters and returns a collection of doses that were taken at the specified point in time from the specified guild.
    /// </summary>
    /// <param name="guild">The guild to get the doses of.</param>
    /// <param name="dto">The date to get the doses from.</param>
    /// <returns>A reference to the new collection of doses.</returns>
    public static IEnumerable<SubstanceUserDoseInfo> GetDosesAt([NotNull] IGuild guild, DateTimeOffset dto)
    {
      return GetDoses(guild).Where(x => x.TakenAt == dto);
    }

    /// <summary>
    /// Filters and returns a collection of doses that were taken after the specified point in time from the specified guild.
    /// </summary>
    /// <param name="guild">The guild to get the doses of.</param>
    /// <param name="after">The date to get the doses after.</param>
    /// <returns>A reference to the new collection of doses.</returns>
    public static IEnumerable<SubstanceUserDoseInfo> GetDosesAfter([NotNull] IGuild guild, DateTimeOffset after)
    {
      return GetDoses(guild).Where(x => x.TakenAt > after);
    }

    /// <summary>
    /// Filters and returns a collection of doses that were taken before the specified point in time from the specified guild.
    /// </summary>
    /// <param name="guild">The guild to get the doses of.</param>
    /// <param name="before">The date to get the doses before.</param>
    /// <returns>A reference to the new collection of doses.</returns>
    public static IEnumerable<SubstanceUserDoseInfo> GetDosesBefore([NotNull] IGuild guild, DateTimeOffset before)
    {
      return GetDoses(guild).Where(x => x.TakenAt < before);
    }
  }

}
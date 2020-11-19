using System;

using Discord.WebSocket;

using Experimental.API;
using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.Common.Builders
{
  /// <summary>
  /// Represents a builder that builds objects of type <see cref="Community"/>.
  /// </summary>
  public class CommunityBuilder : BuilderBase<Community>
  {
    /// <summary>
    /// Gets or sets the identifier of the guild.
    /// </summary>
    public SocketGuild Guild { get; set; }

    /// <summary>
    /// Sets the guild.
    /// </summary>
    /// <param name="guild">The new guild value.</param>
    /// <returns>A reference to the current instance.</returns>
    public CommunityBuilder WithGuild([NotNull] SocketGuild guild)
    {
      Guild = guild;
      return this;
    }

    /// <summary>
    /// Builds the object and returns the result.
    /// </summary>
    /// <returns>A reference to the built object.</returns>
    public override Community Build()
    {
      return new Community(Guid.NewGuid())
      {
        Guild = Guild
      };
    }
  }

}
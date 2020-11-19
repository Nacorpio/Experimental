using System;

using Discord;

using Experimental.API;
using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.Common.Builders
{

  public class GroupSessionBuilder : BuilderBase<GroupSession>
  {
    public ITextChannel Channel { get; set; }
    public DateTimeOffset? StartedAt { get; set; }

    public GroupSessionBuilder WithChannel([NotNull] ITextChannel value)
    {
      Channel = value;
      return this;
    }

    public GroupSessionBuilder WithStartedAt(DateTimeOffset value)
    {
      StartedAt = value;
      return this;
    }

    public override GroupSession Build()
    {
      return new GroupSession(Guid.NewGuid())
      {
        Channel = Channel,
        StartedAt = StartedAt
      };
    }
  }

}
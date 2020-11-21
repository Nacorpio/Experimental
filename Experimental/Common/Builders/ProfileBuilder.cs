using System;

using Discord;

using Experimental.API;
using Experimental.Common.Objects;

namespace Experimental.Common.Builders
{

  public class ProfileBuilder : BuilderBase<Profile>
  {
    public ulong? UserId { get; set; }

    public ProfileBuilder WithUser(IUser user)
    {
      UserId = user.Id;
      return this;
    }

    public override Profile Build()
    {
      return new Profile(Guid.NewGuid())
      {
        UserId = UserId
      };
    }
  }

}
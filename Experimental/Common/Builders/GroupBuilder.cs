using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;

using Experimental.Common.Objects;

namespace Experimental.Common.Builders
{

  public class GroupBuilder : AsyncBuilderBase<Group>
  {
    public GroupBuilder()
    {
      IsMentionable = false;
      Permissions   = GuildPermissions.None;
      Color         = Color.Default;
    }

    public string Name { get; set; }
    public string Summary { get; set; }
    public string ImageUrl { get; set; }

    public bool IsMentionable { get; set; }

    public Color Color { get; set; }
    public GuildPermissions Permissions { get; set; }

    public List<IUser> Members { get; set; }

    public GroupBuilder WithName(string value)
    {
      Name = value;
      return this;
    }

    public GroupBuilder WithSummary(string value)
    {
      Summary = value;
      return this;
    }

    public GroupBuilder WithImageUrl(string value)
    {
      ImageUrl = value;
      return this;
    }

    public GroupBuilder WithIsMentionable(bool value)
    {
      IsMentionable = value;
      return this;
    }

    public GroupBuilder WithColor(Color value)
    {
      Color = value;
      return this;
    }

    public GroupBuilder WithPermissions(GuildPermissions value)
    {
      Permissions = value;
      return this;
    }

    public override async Task<Group> Build()
    {
      var group = new Group(Guid.NewGuid())
      {
        Name          = Name,
        Summary       = Summary,
        ImageUrl      = ImageUrl,
        IsMentionable = IsMentionable,
        Color         = Color,
        Permissions   = Permissions
      };

      foreach (var member in Members)
      {
        await group.AddMemberAsync(member);
      }

      return group;
    }
  }

}
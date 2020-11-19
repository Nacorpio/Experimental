using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using JetBrains.Annotations;

namespace Experimental.Common.Objects
{

  /// <summary>
  /// Represents a managed Discord role.
  /// </summary>
  public class Group : ManagedObject
  {
    public const string InfoImageUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/160/twitter/233/information-source_2139.png";
    public const string KickImageUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/160/twitter/233/no-entry_26d4.png";

    public Group(Guid guid) : base(guid)
    {
    }

    protected Group()
    {
      Role = null;
      Color = Color.Default;
      Permissions = GuildPermissions.None;
    }

    public string Name { get; internal set; }
    public string Summary { get; internal set; }
    public string ImageUrl { get; internal set; }

    public bool IsMentionable { get; internal set; }

    public Color Color { get; internal set; }
    public SocketRole Role { get; internal set; }
    public GuildPermissions Permissions { get; internal set; }

    public List<IUser> Members { get; internal set; }

    public async Task AddMemberAsync([NotNull] IUser user)
    {
      if (!(user is SocketGuildUser sgu))
      {
        return;
      }

      await sgu.AddRoleAsync(Role);
      Members.Add(user);
    }

    public async Task KickMemberAsync([NotNull] IUser user, string reason = null)
    {
      if (!(user is SocketGuildUser sgu))
      {
        return;
      }

      await sgu.RemoveRoleAsync(Role);

      await user.SendMessageAsync(
        null, false,
        new EmbedBuilder()
         .WithAuthor(Guild.Name, Guild.IconUrl)
         .WithTitle($"You have been kicked from group {Role?.Mention}.")
         .WithDescription($"All of the perks associated with the group membership have been revoked. "
                          + $"Contact one of the staff members of the group for more information.")
         .AddField("Reason", "- There was no reason specified by the member.")
         .WithCurrentTimestamp()
         .Build());
    }

    public override async Task InitializeAsync()
    {
      await base.InitializeAsync();

      if (Role == null)
      {
        Role = (SocketRole)await Guild.CreateRoleAsync(Name, Permissions, Color, true, IsMentionable);

        foreach (var member in Members)
        {
          await AddMemberAsync(member);
        }
      }
    }

    public override async Task DestroyAsync()
    {
      await base.DestroyAsync();

      if (Role != null)
      {
        await Role.DeleteAsync();
      }
    }

    public async Task PrintAsync([NotNull] ITextChannel _)
    {
      await _
       .SendMessageAsync(
          null, false,
          new EmbedBuilder()
           .WithAuthor(Name)
           .WithDescription(Summary)
           .WithImageUrl(ImageUrl)
           .WithThumbnailUrl(InfoImageUrl)
           .Build());
    }
  }

}
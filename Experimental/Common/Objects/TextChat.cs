using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;

using JetBrains.Annotations;

namespace Experimental.Common.Objects
{

  /// <summary>
  /// Represents a managed text channel.
  /// </summary>
  public partial class TextChat : ManagedObject
  {

    internal TextChat(Guid guid) : base(guid)
    {
      Rules = new List<Rule>();
    }

    public TextChat()
    {
      Channel = null;
      Rules = null;
    }

    public string Name { get; internal set; }

    public ITextChannel Channel { get; internal set; }
    public List<Rule> Rules { get; internal set; }


    public async void AllowUser([NotNull] IUser user)
    {
      await Channel.AddPermissionOverwriteAsync(user, OverwritePermissions.AllowAll(Channel));
    }

    public async void DenyUser([NotNull] IUser user)
    {
      await Channel.AddPermissionOverwriteAsync(user, OverwritePermissions.DenyAll(Channel));
    }


    public virtual async Task PrintRulesAsync([NotNull] ITextChannel channel)
    {
      var eb = new EmbedBuilder()
       .WithDescription($"Remember that these rules apply to {Channel.Mention} specifically.")
       .WithAuthor("These are the rules for this channel", Emotes.TextChatAuthorIconUrl)
       .WithColor(Color.Blue)
       .WithThumbnailUrl(Emotes.TextChatRulesIconUrl)
       .WithCurrentTimestamp();

      for (var i = 0; i < Rules.Count; i++)
      {
        eb.AddField($"{Emotes.OneToTen[i]} {Rules[i].Title}", Rules[i].Summary);
      }

      await channel
       .SendMessageAsync(null, false, eb
       .Build());
    }


    public override async Task InitializeAsync()
    {
      await base.InitializeAsync();

      if (Channel == null)
      {
        Channel = await Guild.CreateTextChannelAsync(Name);
      }
    }

    public override async Task DestroyAsync()
    {
      await base.DestroyAsync();

      if (Channel != null)
      {
        await Channel.DeleteAsync();
      }
    }

  }

}
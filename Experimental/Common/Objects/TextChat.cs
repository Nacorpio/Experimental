using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using JetBrains.Annotations;

namespace Experimental.Common.Objects
{

  /// <summary>
  /// Represents a managed text channel.
  /// </summary>
  public partial class TextChat : ManagedObject
  {
    public TextChat([NotNull] ITextChannel channel) : base(Guid.NewGuid())
    {
      Members = new Dictionary<ulong, SocketGuildUser>();
      Channel = channel;

      Name = channel.Name;
      Topic = channel.Topic;
      CategoryId = channel.CategoryId;
      IsNsfw = channel.IsNsfw;
      Position = channel.Position;
      SlowModeInterval = channel.SlowModeInterval;
    }

    public TextChat()
    {
      Channel = null;
      Members = null;
    }

    public string Name { get; internal set; }
    public string Topic { get; internal set; }

    public ulong? CategoryId { get; internal set; }
    public bool IsNsfw { get; internal set; }

    public int Position { get; internal set; }
    public int SlowModeInterval { get; internal set; }

    public ITextChannel Channel { get; internal set; }

    public Dictionary<ulong, SocketGuildUser> Members { get; internal set; }

    public async void KickMember([NotNull] IUser user, string reason = null)
    {
      _Remove(user);

      await Channel
       .SendMessageAsync(
          null, false, new EmbedBuilder()
           .WithAuthor(user)
           .WithTitle("A member was kicked from the chat")
           .WithDescription($"User {user.Mention} was kicked")
           .AddField("Message", reason ?? "There was no reason provided.")
           .WithThumbnailUrl(Emotes.EmoteCollision)
           .WithCurrentTimestamp()
           .Build());
    }

    public async void AddMember([NotNull] IUser user)
    {
      _Add(user);

      await Channel
       .SendMessageAsync(
          null, false, new EmbedBuilder()
           .WithAuthor(user)
           .WithTitle("A new member has been added to the chat 👀")
           .WithDescription($"User {user.Mention} has joined")
           .WithThumbnailUrl(Emotes.EmoteWavingHand)
           .WithCurrentTimestamp()
           .Build());
    }

    public async void MemberLeave([NotNull] IUser user)
    {
      _Remove(user);

      await Channel
       .SendMessageAsync(
          null, false, new EmbedBuilder()
           .WithAuthor(user)
           .WithTitle("A user left the chat")
           .WithDescription($"The mysterious {user.Mention} left no message behind.")
           .WithThumbnailUrl(Emotes.EmoteDashingAway)
           .WithCurrentTimestamp()
           .Build());
    }

    internal async void _Add([NotNull] IUser user)
    {
      if (Members.ContainsKey(user.Id))
      {
        return;
      }

      await Channel.AddPermissionOverwriteAsync(user, OverwritePermissions.AllowAll(Channel));
      Members.Add(user.Id, (SocketGuildUser)user);
    }

    internal async void _Remove([NotNull] IUser user)
    {
      if (!Members.ContainsKey(user.Id))
      {
        return;
      }

      await Channel.AddPermissionOverwriteAsync(user, OverwritePermissions.DenyAll(Channel));
      Members.Remove(user.Id);
    }

    public override async Task InitializeAsync()
    {
      await base.InitializeAsync();

      if (Channel == null)
      {
        Channel = await Guild.CreateTextChannelAsync(Name);

        await Channel.ModifyAsync(
          x =>
          {
            x.Name = Name;
            x.Topic = Topic;
            x.Position = Position;
            x.IsNsfw = IsNsfw;
            x.CategoryId = CategoryId;
          }
        );
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

    internal async Task OnMessageReceived(SocketMessage message)
    {
      var content = message.Content;

      if (!content[0].Equals('+'))
      {
        // The message is not a command.
        return;
      }

      content = content.Remove(0, 1);

      var parts = content.Split(' ');
      var arguments = parts.Except(new[] { parts.First() }).ToArray();

      // The part at índex zero is the command name.
      // Demonstration: +kick <user> <reason>
      //                 ^0   ^1     ^2
      switch (parts[0])
      {
        // Kicks the user.
        // Example: +kick CoRe Being an asshole
        case "kick":
          {
            //if (arguments.Length < 1)
            //{
            //  // Incorrect number of arguments.
            //  return;
            //}

            //string targetUser = arguments[0];
            //string reason = null;

            //if (arguments.Length == 2)
            //{
            //  // We've got the 'reason' argument as well.
            //  reason = arguments[1];
            //}

            
            break;
          }

        // Signifies that the user wants to leave.
        // Example: +leave
        case "leave":
          {
            break;
          }
      }
    }
  }

}
using System;
using System.Threading.Tasks;

using Discord;
using Discord.Rest;
using Discord.WebSocket;

using Experimental.Common.Builders;

using JetBrains.Annotations;

namespace Experimental.Common.Objects
{

  public class MessageReaction : ManagedObject
  {
    public static MessageReaction CreateInstance
    ([NotNull] IGuild guild, [NotNull] Action<MessageReactionBuilder> _) =>
      Global
     .GetStore(guild)
       .CreateObject<MessageReaction, MessageReactionBuilder>(_);

    public static MessageReaction CreateInstance
    (
      [NotNull] IGuild guild,
      [NotNull] SocketGuildUser user,
      [NotNull] RestUserMessage message,
      [NotNull] SocketGuildChannel channel) =>
      CreateInstance(
        guild, x => x
         .WithUser(user)
         .WithMessage(message)
         .WithChannel(channel)
      );

    public new static MessageReaction Null => new MessageReaction();

    internal MessageReaction(Guid guid) : base(guid)
    {
    }

    public MessageReaction()
    {
    }

    public SocketGuildUser User { get; internal set; }
    public RestUserMessage Message { get; internal set; }
    public SocketGuildChannel Channel { get; internal set; }
    public SocketReaction Reaction { get; internal set; }

    public override async Task DestroyAsync()
    {
      await base.DestroyAsync();
      await Message.RemoveReactionAsync(Reaction.Emote, User);
    }
  }

}
using System;

using Discord.Rest;
using Discord.WebSocket;

using Experimental.API;
using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.Common.Builders
{

  public class MessageReactionBuilder : BuilderBase<MessageReaction>
  {
    public SocketGuildUser User { get; set; }
    public RestUserMessage Message { get; set; }
    public SocketGuildChannel Channel { get; set; }
    public SocketReaction Reaction { get; set; }

    public MessageReactionBuilder WithUser([NotNull] SocketGuildUser value)
    {
      User = value;
      return this;
    }

    public MessageReactionBuilder WithMessage([NotNull] RestUserMessage value)
    {
      Message = value;
      return this;
    }

    public MessageReactionBuilder WithChannel([NotNull] SocketGuildChannel value)
    {
      Channel = value;
      return this;
    }

    public MessageReactionBuilder WithReaction([NotNull] SocketReaction value)
    {
      Reaction = value;
      return this;
    }

    public override MessageReaction Build()
    {
      return new MessageReaction(Guid.NewGuid())
      {
        User     = User,
        Message  = Message,
        Channel  = Channel,
        Reaction = Reaction
      };
    }
  }

}
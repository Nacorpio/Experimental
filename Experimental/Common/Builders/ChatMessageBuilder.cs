using System;

using Discord;

using Experimental.API;
using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.Common.Builders
{

  public class ChatMessageBuilder : BuilderBase<ChatMessage>
  {
    public string Text { get; set; }
    public bool IsTTS { get; set; }

    public Embed Embed { get; set; }

    public ChatMessageBuilder WithText(string value)
    {
      Text = value;
      return this;
    }

    public ChatMessageBuilder WithTTS(bool value)
    {
      IsTTS = value;
      return this;
    }

    public ChatMessageBuilder WithEmbed(Embed value)
    {
      Embed = value;
      return this;
    }

    public ChatMessageBuilder WithEmbed([NotNull] Action<EmbedBuilder> _)
    {
      var eb = new EmbedBuilder();
      _(eb);

      return WithEmbed(eb.Build());
    }

    public ChatMessageBuilder WithEmbed([NotNull] EmbedBuilder builder) => WithEmbed(builder.Build());

    public override ChatMessage Build()
    {
      return new ChatMessage(Guid.NewGuid())
      {
        Text  = Text,
        IsTTS = IsTTS,
        Embed = Embed
      };
    }
  }

}
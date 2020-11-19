using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Rest;
using Discord.WebSocket;

using Experimental.Common.Builders;

using JetBrains.Annotations;

namespace Experimental.Common.Objects
{

  public class ChatMessage : ManagedObject
  {
    public static ChatMessage CreateInstance
    (
      [NotNull] IGuild guild,
      Action<ChatMessageBuilder> _) => Global
       .GetStore(guild)
       .CreateObject<ChatMessage, ChatMessageBuilder>(_);

    public static ChatMessage CreateInstance
    (
      [NotNull] IGuild guild,
      [NotNull] ITextChannel channel,
      [CanBeNull] string text,
      bool isTTS,
      [NotNull] Action<EmbedBuilder> _)
    {
      var instance = CreateInstance(
        guild,
        x => x
         .WithText(text)
         .WithTTS(isTTS)
         .WithEmbed(_)
      );

      instance.Channel = (SocketTextChannel) channel;

      return instance;
    }

    public new static ChatMessage Null => new ChatMessage();

    private readonly Dictionary<ulong, MessageReaction> _reactions;

    internal ChatMessage(Guid guid) : base(guid)
    {
      _reactions = new Dictionary<ulong, MessageReaction>();
    }

    public ChatMessage()
    {
      _reactions = null;

      Channel = null;
      Message = null;
    }

    public string Text { get; internal set; }
    public bool IsTTS { get; internal set; }

    public Embed Embed { get; internal set; }

    public SocketGuildUser User { get; internal set; }
    public SocketTextChannel Channel { get; internal set; }
    public RestUserMessage Message { get; internal set; }

    public IReadOnlyDictionary<ulong, MessageReaction> Reactions => _reactions;


    public async Task PinAsync()
    {
      await Message.PinAsync();
    }

    public async Task UnpinAsync()
    {
      await Message.UnpinAsync();
    }


    internal void AddReaction([NotNull] SocketReaction sr)
    {
      _reactions.Add(sr.UserId, MessageReaction.CreateInstance(
        Guild, x => x
         .WithChannel(Channel)
         .WithMessage(Message)
         .WithUser(User)
         .WithReaction(sr)
      ));
    }


    public override async Task InitializeAsync()
    {
      if (Message == null)
      {
        Message = await Channel.SendMessageAsync(Text, IsTTS, Embed);

        Text = Message.Content;
        IsTTS = Message.IsTTS;

        User = (SocketGuildUser)Message.Author;
        Channel = (SocketTextChannel)Message.Channel;
      }
    }

    public override async Task DestroyAsync()
    {
      await base.DestroyAsync();

      if (Message != null)
      {
        await Message.DeleteAsync();
      }
    }


    internal async Task OnMessageUpdated
      (Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
    {
      if (!(arg2 is SocketUserMessage sum))
      {
        return;
      }

      if (!string.Equals(Text, arg2.Content))
      {
        Text = arg2.Content;
      }

      Embed = sum.Embeds.FirstOrDefault();
    }

    internal async Task OnMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
    {
      var sum = (SocketUserMessage)await arg1.GetOrDownloadAsync();

      if (sum == null)
      {
        return;
      }

      foreach (var reaction in Reactions)
      {
        Store.DestroyObject(reaction.Value);
      }

      _reactions.Clear();

      Message = null;
      Store.DestroyObject(this);
    }
  }

}
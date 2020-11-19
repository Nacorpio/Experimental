using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;

using Experimental.Common.Builders;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnitsNet;

namespace Experimental.Common.Objects
{

  public class GroupSession : ManagedObject
  {
    public static GroupSession CreateInstance
      ([NotNull] IGuild guild, [NotNull] Action<GroupSessionBuilder> _) =>
      Global.GetStore(guild).CreateObject<GroupSession, GroupSessionBuilder>(_);

    public static GroupSession CreateInstance
      ([NotNull] IGuild guild, ITextChannel channel) =>
      CreateInstance(
        guild,
        x => x
         .WithChannel(channel)
      );

    public new static GroupSession Null => new GroupSession();

    public GroupSession(Guid guid) : base(guid)
    {
      Doses = new List<SubstanceUserDoseInfo>();
      Participants = new List<IUser>();
    }

    public GroupSession()
    {
      Channel = null;
      Doses = null;
      Participants = null;
    }


    [JsonProperty("channel_id")]
    public ITextChannel Channel { get; internal set; }


    [JsonProperty("doses")]
    public List<SubstanceUserDoseInfo> Doses { get; internal set; }

    [JsonProperty("participants")]
    public List<IUser> Participants { get; internal set; }


    [JsonProperty("started_at")]
    public DateTimeOffset? StartedAt { get; internal set; }

    [JsonProperty("ended_at")]
    public DateTimeOffset? EndedAt { get; internal set; }


    public bool IsStarted { get; internal set; }
    public bool HasEnded { get; internal set; }


    public async Task BeginAsync()
    {
      if (IsStarted)
      {
        return;
      }

      StartedAt = DateTimeOffset.Now;
      IsStarted = true;

      await Channel.SendMessageAsync(
        null, false,
        new EmbedBuilder()
         .WithAuthor(Guild.Name, Guild.IconUrl)
         .WithTitle("The group session has started!")
         .WithDescription("The members of this group will now enjoy themselves with each other virtually!")
         .WithThumbnailUrl(Emotes.EmoteGroupSilhouette)
         .WithFooter("Remember to stay hydrated", Emotes.EmoteDropletUrl)
         .AddField("📄 **Notes**", 
                   "In order to log that you take a substance you have to use a command. "
                 + "The command will depend on the route of administration that you use.\n\n"
                 + "If you for instance want to *snort* cocaine, you type:\n`+snort cocaine 0,1g`\n\n"
                 + "If you are one of the few who take it up the bum, you type:\n`+boof mdma 0,1g`")
         .WithCurrentTimestamp()
         .Build()
      );
    }

    public async Task EndAsync()
    {
      if (!IsStarted)
      {
        return;
      }

      IsStarted = false;
      HasEnded = true;

      EndedAt = DateTimeOffset.Now;

      var duration = EndedAt - StartedAt;

      await Channel.SendMessageAsync(
        null, false,
        new EmbedBuilder()
         .WithAuthor(Guild.Name, Guild.IconUrl)
         .WithTitle("The group session has now ended!")
         .WithDescription($"The group session lasted for **{duration?.Days} days, {duration?.Hours} hours, {duration?.Minutes} minutes and {duration?.Seconds} seconds**")
         .WithThumbnailUrl(Emotes.EmoteGroupSilhouette)
         .WithFooter("Remember to stay hydrated", Emotes.EmoteDropletUrl)
         .WithCurrentTimestamp()
         .Build()
      );
    }


    public void AddDose([NotNull] Action<SubstanceUserDoseBuilder> _)
    {
      var store = Global.GetStore(Guild);
      var dose = store.CreateObject<SubstanceUserDoseInfo, SubstanceUserDoseBuilder>(_);

      Doses.Add(dose);
    }

    public async void AddDoseAsync
    (
      [NotNull] IUser user,
      [NotNull] ITextChannel channel,
      [NotNull] Substance substance,
      DateTimeOffset takenAt,
      Mass amount)
    {
      AddDose(x => x
               .WithUser(user)
               .WithSubstance(substance)
               .WithTakenAt(takenAt)
               .WithAmount(amount));

      await channel.SendMessageAsync(
        null, false,
        new EmbedBuilder()
         .WithAuthor(Guild.Name, Guild.IconUrl)
         .WithTitle($"User {user.Mention} has taken a dose of {substance.DisplayName.ToLower()}")
         .WithDescription($"The amount that was taken is **{amount.ToString()}**")
         .WithThumbnailUrl(Emotes.EmoteGroupSilhouette)
         .WithFooter("Remember to stay hydrated", Emotes.EmoteDropletUrl)
         .WithCurrentTimestamp()
         .Build()
      );
    }


    public void AddParticipant([NotNull] IUser user)
    {
      if (HasParticipant(user))
      {
        return;
      }

      Participants.Add(user);
    }

    public bool RemoveParticipant([NotNull] IUser user)
    {
      return HasParticipant(user) && Participants.Remove(user);
    }

    public bool HasParticipant([NotNull] IUser user)
    {
      return Participants.Any(x => x.Id == user.Id);
    }


    public override JObject ToJson()
    {
      var _ = base.ToJson();

      _["channel_id"] = Channel.Id;
      _["doses"] = new JArray(Doses.Select(x => x.ToJson()));
      _["participants"] = new JArray(Participants.Select(x => x.Id));

      return _;
    }

    public override async void FromJson(JObject json)
    {
      base.FromJson(json);

      Channel = await Guild.GetTextChannelAsync(json.Value<ulong>("channel_id"));

      Doses = json["doses"]?.Select(
        x =>
        {
          var _ = Global
           .GetStore(Store.Community)
           .CreateObject<SubstanceUserDoseInfo>();

          _.FromJson(x as JObject);

          return _;
        }
      ).ToList();

      Participants = new List<IUser>(
        json.Values<ulong>("participants")
         .Select(x => Global.Client.GetUser(x))
      );
    }
  }

}
using Discord;

using JetBrains.Annotations;

namespace Experimental.Common.Extensions
{

  public static class EmbedExtensions
  {
    public static EmbedBuilder WithGeneralInfo(
      this EmbedBuilder _, [NotNull] IGuildUser guildUser) =>
      _
       .WithAuthor(guildUser.Guild.Name, guildUser.Guild.IconUrl)
       .WithWaterInfo()
       .WithUserInfo(guildUser)
       .WithCurrentTimestamp();

    public static EmbedBuilder WithGuildInfo(
      this EmbedBuilder _, [NotNull] IGuild guild) =>
      _.WithAuthor(guild.Name, guild.IconUrl);

    public static EmbedBuilder WithWaterInfo(
      this EmbedBuilder _) =>
      _
       .WithFooter("Remember to stay hydrated", Emotes.EmoteDropletUrl)
       .WithCurrentTimestamp();

    public static EmbedBuilder WithUserInfo(
      this EmbedBuilder _, [NotNull] IGuildUser guildUser) =>
      _
       .WithAuthor(guildUser.Guild.Name, guildUser.Guild.IconUrl)
       .WithCurrentTimestamp();
  }

}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;

using Experimental.Common.Builders;

using JetBrains.Annotations;

namespace Experimental.Common.Objects
{

  public partial class TextChat
  {
    public sealed class Rule
    {
      internal Rule([NotNull] string title, [NotNull] string summary)
      {
        Title = title;
        Summary = summary;
      }

      public string Title { get; internal set; }
      public string Summary { get; internal set; }
    }

    public static TextChat CreateInstance
      ([NotNull] IGuild guild, [NotNull] Action<TextChatBuilder> _) =>
      Global.GetStore(guild).CreateObject<TextChat, TextChatBuilder>(_);

    public static async Task<TextChat> CreateInstanceAsync
    (
      [NotNull] IGuild guild,
      [NotNull] string name,
      string topic = null,
      bool isNsfw = false,
      int slowModeInterval = 0,
      Dictionary<string, string> rules = null)
    {
      var instance = CreateInstance(
        guild,
        x =>
        {
          if (rules != null)
            x.AddRules(
              rules
            );
        }
      );

      await instance.Channel
       .ModifyAsync(
          x =>
          {
            x.Name             = name;
            x.Topic            = topic;
            x.IsNsfw           = isNsfw;
            x.SlowModeInterval = slowModeInterval;
          }
        );

      return instance;
    }

  }

}
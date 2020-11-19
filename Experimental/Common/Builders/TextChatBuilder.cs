using System;
using System.Collections.Generic;

using Experimental.API;

using JetBrains.Annotations;

using TextChat = Experimental.Common.Objects.TextChat;

namespace Experimental.Common.Builders
{

  public class TextChatBuilder : BuilderBase<TextChat>
  {
    public TextChatBuilder()
    {
      Rules = new List<TextChat.Rule>();
    }

    public string Name { get; set; }
    public string Topic { get; set; }

    public bool IsNsfw { get; set; }
    public ulong? CategoryId { get; set; }

    public int Position { get; set; }
    public int SlowModeInterval { get; set; }

    public List<TextChat.Rule> Rules { get; set; }


    public TextChatBuilder WithName(string value)
    {
      Name = value;
      return this;
    }

    public TextChatBuilder WithTopic(string value)
    {
      Topic = value;
      return this;
    }


    public TextChatBuilder WithCategoryId(ulong value)
    {
      CategoryId = value;
      return this;
    }

    public TextChatBuilder WithPosition(int value)
    {
      Position = value;
      return this;
    }

    public TextChatBuilder WithSlowModeInterval(int value)
    {
      SlowModeInterval = value;
      return this;
    }


    public TextChatBuilder WithNsfw(bool value)
    {
      IsNsfw = value;
      return this;
    }


    public TextChatBuilder AddRule([NotNull] string title, [NotNull] string summary)
    {
      Rules.Add(new TextChat.Rule(title, summary));
      return this;
    }

    public TextChatBuilder AddRules([NotNull] Dictionary<string, string> rules)
    {
      foreach (var rule in rules)
      {
        AddRule(rule.Key, rule.Value);
      }

      return this;
    }


    public override TextChat Build()
    {
      return new TextChat(Guid.NewGuid())
      {
        Name = Name,
        Rules = Rules
      };
    }
  }

}
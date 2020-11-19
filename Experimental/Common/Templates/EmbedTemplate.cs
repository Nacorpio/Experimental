using System;
using System.Collections.Generic;

using Discord;

using Newtonsoft.Json.Linq;

namespace Experimental.Common.Templates
{
  public class EmbedTemplate : TemplateBase
  {
    public EmbedTemplate(Guid guid) : base(guid)
    {
    }

    protected EmbedTemplate()
    {
      Color = Color.Default;
      Timestamp = DateTimeOffset.MinValue;
    }

    public string Title { get; internal set; }
    public string Description { get; internal set; }

    public string Url { get; internal set; }
    public string ImageUrl { get; internal set; }
    public string ThumbnailUrl { get; internal set; }

    public string FooterText { get; internal set; }
    public string FooterIconUrl { get; internal set; }

    public Color Color { get; internal set; }
    public DateTimeOffset Timestamp { get; internal set; }

    public List<(string name, object value, bool inline)> Fields { get; internal set; }

    public Embed AsEmbed()
    {
      var eb = new EmbedBuilder()
       .WithTitle(Title)
       .WithDescription(Description)
       .WithImageUrl(ImageUrl)
       .WithThumbnailUrl(ThumbnailUrl)
       .WithUrl(Url)
       .WithColor(Color)
       .WithFooter(FooterText, FooterIconUrl)
       .WithTimestamp(Timestamp);

      foreach (var (name, value, inline) in Fields)
      {
        eb.AddField(name, value, inline);
      }

      return eb.Build();
    }

    public override void FromJson(JObject json)
    {
      base.FromJson(json);

      Title = json.Value<string>("title");
      Description = json.Value<string>("description");
      Url = json.Value<string>("url");
      ImageUrl = json.Value<string>("image_url");
      ThumbnailUrl = json.Value<string>("thumbnail_url");
      FooterText = json.Value<string>("footer_text");
      FooterIconUrl = json.Value<string>("footer_icon_url");
      Color = json.Value<Color>("color");
      Timestamp = json.Value<DateTimeOffset>("timestamp");
      Fields = json.Value<List<(string name, object value, bool inline)>>("fields");
    }

    public override JObject ToJson()
    {
      var _ = base.ToJson();

      _["title"] = Title;
      _["description"] = Description;
      _["url"] = Url;
      _["image_url"] = Title;
      _["thumbnail_url"] = Description;
      _["footer_text"] = Url;
      _["footer_icon_url"] = Title;
      _["color"] = Description;
      _["timestamp"] = Url;
      _["fields"] = new JArray(Fields);

      return _;
    }
  }

}
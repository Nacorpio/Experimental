using System;

using Experimental.API;

using Newtonsoft.Json.Linq;

namespace Experimental.Common.Templates
{

  public abstract class TemplateBase : ITemplate
  {
    protected TemplateBase(Guid guid)
    {
      Guid = guid;
    }

    protected TemplateBase() : this(Guid.Empty)
    {
    }

    public Guid Guid { get; internal set; }

    public string Name { get; internal set; }
    public string DisplayName { get; internal set; }

    public virtual void FromJson(JObject json)
    {
      Guid = json.Value<Guid>("guid");
      Name = json.Value<string>("name");
      DisplayName = json.Value<string>("display_name");
    }

    public virtual JObject ToJson()
    {
      return new JObject
      (
        new JProperty("guid", Guid),
        new JProperty("name", Name),
        new JProperty("display_name", DisplayName)
      );
    }
  }

}
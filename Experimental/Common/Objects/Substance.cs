using System;

using Discord;

using Experimental.Common.Builders;
using Experimental.Common.Objects.Data;
using Experimental.Common.Storage;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using UnitsNet.Units;

namespace Experimental.Common.Objects
{
  public class Substance : ManagedObject
  {
    public new static Substance Null => new Substance();

    public static Substance CreateInstance
      ([NotNull] Action<SubstanceBuilder> _)
    {
      if (Global.Store == null)
      {
        Global.Store = new Store();
      }

      var sb = new SubstanceBuilder();
      _(sb);

      return sb.Build();
    }

    public static Substance CreateInstance
    (
      string name,
      string displayName = null,
      string[] aliases = null,
      string imageUrl = null,
      SubstanceCategory category = SubstanceCategory.Undefined,
      MassUnit baseUnit = MassUnit.Undefined,
      Color? color = null) =>
      CreateInstance(
        x => x
         .WithName(name)
         .WithDisplayName(displayName)
         .WithBaseUnit(baseUnit)
         .WithCategory(category)
         .WithImageUrl(imageUrl)
         .AddAliases(aliases)
      );

    public Substance
    (
      string name = null,
      string displayName = null,
      string description = null,
      string[] aliases = null,
      string imageUrl = null,
      SubstanceCategory category = SubstanceCategory.Undefined,
      MassUnit baseUnit = MassUnit.Undefined,
      Color? color = null) : base(
      Guid.NewGuid()
    )
    {
      Name = name;
      DisplayName = displayName;
      Description = description;
      Aliases = aliases;
      ImageUrl = imageUrl;
      Category = category;
      BaseUnit = baseUnit;
      Color = color;
    }

    public Substance()
    {
      Color = Discord.Color.Default;
      BaseUnit = MassUnit.Undefined;
      Category = SubstanceCategory.Undefined;
    }

    public string Name { get; internal set; }
    public string[] Aliases { get; internal set; }

    public string DisplayName { get; internal set; }
    public string Description { get; internal set; }

    public string ImageUrl { get; internal set; }

    public Color? Color { get; internal set; }
    public MassUnit BaseUnit { get; internal set; }

    public SubstanceCategory Category { get; internal set; }

    public override JObject ToJson()
    {
      var _ = base.ToJson();

      _["name"] = Name;
      _["aliases"] = new JArray((string[])Aliases);
      _["display_name"] = DisplayName;
      _["description"] = Description;
      _["image_url"] = ImageUrl;
      _["color"] = Color?.RawValue;
      _["base_unit"] = (int)BaseUnit;
      _["category"] = (int)Category;

      return _;
    }

    public override void FromJson(JObject json)
    {
      base.FromJson(json);

      Name = json.Value<string>("name");
      Aliases = (string[])json.Values<string>("aliases");
      DisplayName = json.Value<string>("display_name");
      Description = json.Value<string>("description");
      ImageUrl = json.Value<string>("image_url");
      Color = new Color(json.Value<uint>("color"));
      BaseUnit = (MassUnit)json.Value<int>("base_unit");
      Category = (SubstanceCategory)json.Value<int>("category");
    }
  }

}
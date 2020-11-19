using System;

using Discord;

using Experimental.Common.Builders;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using UnitsNet.Units;

namespace Experimental.Common.Objects
{
  public class Substance : ManagedObject
  {
    public new static Substance Null => new Substance();

    public static Substance Create
      ([NotNull] IGuild guild, [NotNull] Action<SubstanceBuilder> _) =>
      Global.GetStore(guild).CreateObject<Substance>(_);

    public static Substance Create
      ([NotNull] IGuild guild, string name, string displayName = null, MassUnit baseUnit = MassUnit.Undefined, SubstanceCategory category = SubstanceCategory.Undefined) =>
      Create(guild, x => x
        .WithName(name)
        .WithDisplayName(displayName)
        .WithBaseUnit(baseUnit)
        .WithCategory(category));

    internal Substance(Guid guid) : base(guid)
    {
    }

    public Substance()
    {
      Color = Color.Default;
      BaseUnit = MassUnit.Undefined;
      Category = SubstanceCategory.Undefined;
    }

    public string Name { get; internal set; }
    public string[] Aliases { get; internal set; }

    public string DisplayName { get; internal set; }
    public string Description { get; internal set; }

    public string ImageUrl { get; internal set; }

    public Color Color { get; internal set; }
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
      _["color"] = Color.RawValue;
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
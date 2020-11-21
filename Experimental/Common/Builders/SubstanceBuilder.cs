using System;
using System.Collections.Generic;

using Discord;

using Experimental.API;
using Experimental.Common.Objects;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet.Units;

namespace Experimental.Common.Builders
{

  public class SubstanceBuilder : BuilderBase<Substance>
  {
    public SubstanceBuilder()
    {
      Aliases = new List<string>();
      Entries = new List<SubstanceEntryData>();
    }

    public string Name { get; set; }
    public List<string> Aliases { get; set; }

    public string DisplayName { get; set; }
    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public Color Color { get; set; }
    public MassUnit BaseUnit { get; set; }

    public SubstanceCategory Category { get; set; }
    public List<SubstanceEntryData> Entries { get; set; }

    public SubstanceBuilder WithName(string value)
    {
      Name = value;
      return this;
    }

    public SubstanceBuilder AddEntry([NotNull] Action<SubstanceEntryDataBuilder> _)
    {
      Entries.Add(Global.Store.CreateObject<SubstanceEntryData, SubstanceEntryDataBuilder>(_));
      return this;
    }

    public SubstanceBuilder AddAlias(string value)
    {
      Aliases.Add(value);
      return this;
    }

    public SubstanceBuilder AddAliases(IEnumerable<string> values)
    {
      if (values == null)
      {
        Aliases = new List<string>();
        return this;
      }

      foreach (var value in values)
      {
        AddAlias(value);
      }

      return this;
    }

    public SubstanceBuilder WithDisplayName(string value)
    {
      DisplayName = value;
      return this;
    }

    public SubstanceBuilder WithDescription(string value)
    {
      Description = value;
      return this;
    }

    public SubstanceBuilder WithImageUrl(string value)
    {
      ImageUrl = value;
      return this;
    }

    public SubstanceBuilder WithColor(Color value)
    {
      Color = value;
      return this;
    }

    public SubstanceBuilder WithBaseUnit(MassUnit value)
    {
      BaseUnit = value;
      return this;
    }

    public SubstanceBuilder WithCategory(SubstanceCategory value)
    {
      Category = value;
      return this;
    }

    /// <summary>
    /// Builds the object and returns the result.
    /// </summary>
    /// <returns>A reference to the built object.</returns>
    public override Substance Build()
    {
      var substance = new Substance(
        Name,
        DisplayName,
        Description,
        Aliases.ToArray(),
        ImageUrl,
        Category,
        BaseUnit,
        Color
      );

      foreach (var data in Entries)
      {
        data.Substance = substance;
      }

      return substance;
    }
  }

}
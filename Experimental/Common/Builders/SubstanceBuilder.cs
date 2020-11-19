using System;
using System.Collections.Generic;

using Discord;

using Experimental.API;
using Experimental.Common.Objects;
using Experimental.Common.Objects.Data;

using UnitsNet.Units;

namespace Experimental.Common.Builders
{

  public class SubstanceBuilder : BuilderBase<Substance>
  {
    public SubstanceBuilder()
    {
      Aliases = new List<string>();
    }

    public string Name { get; set; }
    public List<string> Aliases { get; set; }

    public string DisplayName { get; set; }
    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public Color Color { get; set; }
    public MassUnit BaseUnit { get; set; }
    public SubstanceCategory Category { get; set; }

    public SubstanceBuilder WithName(string value)
    {
      Name = value;
      return this;
    }

    public SubstanceBuilder AddAlias(string value)
    {
      Aliases.Add(value);
      return this;
    }

    public SubstanceBuilder AddAliases(params string[] values)
    {
      Aliases.AddRange(values);
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
      return new Substance(Guid.NewGuid())
      {
        Name = Name,
        Aliases = Aliases.ToArray(),
        DisplayName = DisplayName,
        Description = Description,
        ImageUrl = ImageUrl,
        Color = Color,
        BaseUnit = BaseUnit,
        Category = Category
      };
    }
  }

}
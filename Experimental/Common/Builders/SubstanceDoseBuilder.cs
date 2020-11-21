using System;
using System.Collections.Generic;

using Experimental.API;
using Experimental.Common.Objects;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet.Units;

namespace Experimental.Common.Builders
{

  public class SubstanceEntryDataBuilder : BuilderBase<SubstanceEntryData>
  {
    public SubstanceEntryDataBuilder()
    {
      Levels = new Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo>();
      Durations = new Dictionary<SubstanceDurationType, UnitRange<DurationUnit>>();
    }

    public SubstanceRouteType Route { get; set; }
    public Substance Substance { get; set; }

    public Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo> Levels { get; internal set; }
    public Dictionary<SubstanceDurationType, UnitRange<DurationUnit>> Durations { get; internal set; }

    public SubstanceEntryDataBuilder WithSubstance([NotNull] Substance value)
    {
      Substance = value;
      return this;
    }

    public SubstanceEntryDataBuilder WithRoute([NotNull] SubstanceRouteType value)
    {
      Route = value;
      return this;
    }

    public SubstanceEntryDataBuilder AddDuration(SubstanceDurationType type, UnitRange<DurationUnit> range)
    {
      Durations.Add(type, range);
      return this;
    }

    public SubstanceEntryDataBuilder AddDuration(SubstanceDurationType type, double min, double max, DurationUnit unit)
    {
      return AddDuration(type, new UnitRange<DurationUnit>(min, max, unit));
    }

    public SubstanceEntryDataBuilder AddLevel(SubstanceDoseLevelType type, [NotNull] Action<SubstanceDoseLevelBuilder> _)
    {
      var sdlb = new SubstanceDoseLevelBuilder();
      _(sdlb);

      Levels.Add(type, sdlb.Build());

      return this;
    }

    public SubstanceEntryDataBuilder AddLevel(SubstanceDoseLevelType type, UnitRange<MassUnit> range)
    {
      return
        AddLevel(type, x => x
         .WithDose(range)
         .WithLevel(type));
    }

    public SubstanceEntryDataBuilder AddLevel(SubstanceDoseLevelType type, double min, double max, MassUnit unit)
    {
      return AddLevel(type, new UnitRange<MassUnit>(min, max, unit));
    }

    public override SubstanceEntryData Build()
    {
      return new SubstanceEntryData
      {
        Substance = Substance,
        Route = Route,
        Levels = Levels
      };
    }
  }

}
using System;
using System.Collections.Generic;

using Experimental.Common.Builders;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet.Units;

namespace Experimental.Common.Objects
{

  public class SubstanceEntryData : ManagedObject
  {
    public static SubstanceEntryData CreateInstance([NotNull] Action<SubstanceEntryDataBuilder> _)
      => Global.Store.CreateObject<SubstanceEntryData, SubstanceEntryDataBuilder>(_);

    public static SubstanceEntryData CreateInstance
    (
      [NotNull] Substance substance,
      [NotNull] SubstanceRouteType route,
      [NotNull] Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo> levels
    )
    {
      var instance = CreateInstance(x => x
        .WithSubstance(substance)
        );

      if (instance == null)
      {
        return Null;
      }

      foreach (var level in levels.Values)
      {
        instance.Levels.Add(level.Level, level);
      }

      return instance;
    }

    public new static SubstanceEntryData Null => new SubstanceEntryData();

    public SubstanceEntryData([NotNull] Substance substance, [NotNull] SubstanceRouteType route) : base(Guid.NewGuid())
    {
      Substance = substance;
      Route = route;

      Levels = new Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo>();
    }

    public SubstanceEntryData()
    {
      Levels = null;
      Route = SubstanceRouteType.Undefined;
    }

    public Substance Substance { get; internal set; }
    public SubstanceRouteType Route { get; internal set; }

    public Dictionary<SubstanceDurationType, UnitRange<DurationUnit>> Durations { get; internal set; }
    public Dictionary<SubstanceDoseLevelType, SubstanceDoseLevelInfo> Levels { get; internal set; }
  }

}
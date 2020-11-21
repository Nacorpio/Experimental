using System;

using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet.Units;

namespace Experimental.Common.Builders
{

  public sealed class SubstanceDoseLevelInfo
  {
    public static SubstanceDoseLevelInfo CreateInstance([NotNull] Action<SubstanceDoseLevelBuilder> _)
    {
      var sdlb = new SubstanceDoseLevelBuilder();
      _(sdlb);

      return sdlb.Build();
    }

    public SubstanceDoseLevelInfo
    (
      SubstanceDoseLevelType level,
      UnitRange<MassUnit> doseRange)
    {
      Level = level;
      Dose = doseRange;
    }

    public SubstanceDoseLevelInfo()
    {
      Level = SubstanceDoseLevelType.Undefined;

      Dose = null;
    }

    public SubstanceDoseLevelType Level { get; }
    public UnitRange<MassUnit>? Dose { get; }
  }

}
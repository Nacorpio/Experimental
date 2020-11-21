using System;
using Experimental.API.Objects.Data;

namespace Experimental.Common.Objects.Data
{
  public readonly struct UnitRange<TUnit> : IUnitRange<TUnit>
    where TUnit : Enum
  {
    public UnitRange(double minimum, double maximum, TUnit unit)
    {
      Minimum = minimum;
      Maximum = maximum;
      Unit = unit;
    }

    public double Minimum { get; }
    public double Maximum { get; }

    public TUnit Unit { get; }
  }

}
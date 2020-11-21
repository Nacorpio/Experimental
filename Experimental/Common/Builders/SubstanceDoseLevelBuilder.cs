using Experimental.API;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet.Units;

namespace Experimental.Common.Builders
{

  public class SubstanceDoseLevelBuilder : BuilderBase<SubstanceDoseLevelInfo>
  {
    public SubstanceDoseLevelType Level { get; set; }
    public UnitRange<MassUnit> Dose { get; set; }

    public SubstanceDoseLevelBuilder WithLevel([NotNull] SubstanceDoseLevelType value)
    {
      Level = value;
      return this;
    }

    public SubstanceDoseLevelBuilder WithDose(UnitRange<MassUnit> value)
    {
      Dose = value;
      return this;
    }

    public SubstanceDoseLevelBuilder WithDose(double min, double max, MassUnit unit)
    {
      return WithDose(new UnitRange<MassUnit>(min, max, unit));
    }

    public override SubstanceDoseLevelInfo Build()
    {
      return new SubstanceDoseLevelInfo(Level, Dose);
    }
  }

}
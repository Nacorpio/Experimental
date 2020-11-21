using System;

namespace Experimental.API.Objects.Data
{

  public interface IUnitRange<out TUnit> : IRange<double>
    where TUnit : Enum
  {
    TUnit Unit { get; }
  }

}
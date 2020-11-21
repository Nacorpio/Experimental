using System;

namespace Experimental.API.Objects.Data
{

  public interface IRange<out T>
    where T : IEquatable<T>
  {
    T Minimum { get; }
    T Maximum { get; }
  }

}
using System;

using Experimental.API.Serialization;

namespace Experimental.API
{

  public interface ITemplate : IJsonable
  {
    Guid Guid { get; }

    string Name { get; }
    string DisplayName { get; }
  }

}
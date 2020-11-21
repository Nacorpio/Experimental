using System;
using System.Threading.Tasks;

using Experimental.API.Serialization;

namespace Experimental.API
{

  /// <summary>
  /// Defines functionality for a managed object with a unique identity.
  /// </summary>
  public interface IObject : IJsonable
  {
    /// <summary>
    /// Gets the identifier of the managed object.
    /// </summary>
    Guid Guid { get; }


    /// <summary>
    /// Gets the store in which the managed object is stored.
    /// </summary>
    IStore Store { get; }


    /// <summary>
    /// Gets a value indicating whether the state of the managed object is null.
    /// </summary>
    bool IsNull { get; }


    /// <summary>
    /// Initializes the managed object.
    /// </summary>
    /// <returns>A reference to a task executing this method.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Destroys the managed object.
    /// </summary>
    /// <returns>A reference to a task executing this method.</returns>
    Task DestroyAsync();
  }

}
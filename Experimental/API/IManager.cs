using System;
using System.Collections.Generic;

using Experimental.Common.Objects;

namespace Experimental.API
{
  /// <summary>
  /// Defines functionality for a container storing managed objects.
  /// </summary>
  public interface IManager
  {
    /// <summary>
    /// Gets the total number of managed objects contained in the <see cref="IManager"/>.
    /// </summary>
    int Count { get; }


    /// <summary>
    /// Gets the store in which the manager is contained.
    /// </summary>
    IStore Store { get; }


    /// <summary>
    /// Adds the specified managed object to the <see cref="IManager"/>.
    /// </summary>
    /// <param name="object">The managed object to add.</param>
    void Add(ManagedObject @object);

    /// <summary>
    /// Adds a collection of managed objects to the <see cref="IManager"/>.
    /// </summary>
    /// <param name="objects"></param>
    void AddRange(IEnumerable <ManagedObject> objects);


    /// <summary>
    /// Removes the specified managed object from the <see cref="IManager"/>.
    /// </summary>
    /// <param name="object">The managed object to remove.</param>
    /// <returns><c>true</c> if the managed object was removed; otherwise, <c>false</c>.</returns>
    bool Remove(ManagedObject @object);


    /// <summary>
    /// Returns a value indicating whether the specified managed object is contained in the <see cref="IManager"/>.
    /// </summary>
    /// <param name="object">The managed object to find.</param>
    /// <returns><c>true</c> if the managed object was found; otherwise, <c>false</c>.</returns>
    bool Contains(ManagedObject @object);


    /// <summary>
    /// Clears the <see cref="IManager"/> of all managed objects.
    /// </summary>
    void Clear();
  }

  /// <summary>
  /// Defines functionality for a container storing managed objects of type <see cref="TObject"/>.
  /// </summary>
  /// <typeparam name="TObject">The managed object type.</typeparam>
  public interface IManager<TObject> : IManager, IEnumerable<TObject>
    where TObject : ManagedObject
  {
    /// <summary>
    /// Gets a managed object with the specified unique identifier.
    /// </summary>
    /// <param name="guid">The unique identifier of the managed object to get.</param>
    /// <returns>A reference to the managed object, if found; otherwise, <c>null</c>.</returns>
    TObject this[Guid guid] { get; }


    /// <summary>
    /// Adds the specified managed object to the <see cref="IManager{TObject}"/>.
    /// </summary>
    /// <param name="object">The managed object to add.</param>
    void Add(TObject @object);


    /// <summary>
    /// Removes the specified managed object from the <see cref="IManager{TObject}"/>.
    /// </summary>
    /// <param name="object">The managed object to remove.</param>
    /// <returns><c>true</c> if the object was removed; otherwise, <c>false</c>.</returns>
    bool Remove(TObject @object);

    /// <summary>
    /// Returns a value indicating whether the specified managed object is contained in the <see cref="IManager{TObject}"/>.
    /// </summary>
    /// <param name="object">The managed object to find.</param>
    /// <returns><c>true</c> if the object was found; otherwise, <c>false</c>.</returns>
    bool Contains(TObject @object);


    /// <summary>
    /// Finds a managed object with the specified identifier.
    /// </summary>
    /// <param name="objectGuid">The identifier of the managed object to find.</param>
    /// <param name="object">The found managed object.</param>
    /// <returns><c>true</c> if the object was found; otherwise, <c>false</c>.</returns>
    bool TryGetObject(Guid objectGuid, out IObject @object);
  }

}
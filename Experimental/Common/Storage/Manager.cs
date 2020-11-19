using System;
using System.Collections;
using System.Collections.Generic;

using Experimental.API;
using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.Common.Storage
{
  /// <summary>
  /// Represents a manager of managed objects.
  /// </summary>
  /// <typeparam name="TObject">The managed object type.</typeparam>
  public class Manager<TObject> : IManager<TObject> where TObject : ManagedObject, new()
  {
    private readonly Dictionary<Guid, TObject> _objects;

    /// <summary>
    /// Initializes a new instance of the <see cref="Manager{TObject}"/> class.
    /// </summary>
    /// <param name="store">The store.</param>
    public Manager([NotNull] IStore store)
    {
      _objects = new Dictionary<Guid, TObject>();
      Store = store;
    }


    /// <summary>
    /// Gets a managed object with the specified unique identifier.
    /// </summary>
    /// <param name="guid">The unique identifier of the managed object to get.</param>
    /// <returns>A reference to the managed object, if found; otherwise, <c>null</c>.</returns>
    public TObject this[Guid guid] => _objects.TryGetValue(guid, out var @object) ? @object : new TObject();


    /// <summary>
    /// Gets the total number of managed objects contained in the <see cref="IManager"/>.
    /// </summary>
    public int Count => _objects.Count;

    /// <summary>
    /// Gets the store in which the <see cref="IManager"/> is contained.
    /// </summary>
    public IStore Store { get; }

    /// <summary>
    /// Adds the specified managed object to the <see cref="IManager{TObject}"/>.
    /// </summary>
    /// <param name="object">The managed object to add.</param>
    public void Add(TObject @object)
    {
      if (Contains(@object))
      {
        return;
      }

      _objects.Add(@object.Guid, @object);
    }

    /// <summary>
    /// Adds the specified managed object to the <see cref="IManager"/>.
    /// </summary>
    /// <param name="object">The managed object to add.</param>
    public void Add(ManagedObject @object)
    {
      Add(@object as TObject);
    }

    /// <summary>
    /// Removes the specified managed object from the <see cref="IManager{TObject}"/>.
    /// </summary>
    /// <param name="object">The managed object to remove.</param>
    /// <returns><c>true</c> if the object was removed; otherwise, <c>false</c>.</returns>
    public bool Remove(TObject @object)
    {
      return Contains(@object) && _objects.Remove(@object.Guid);
    }

    /// <summary>
    /// Removes the specified managed object from the <see cref="IManager"/>.
    /// </summary>
    /// <param name="object">The managed object to remove.</param>
    /// <returns><c>true</c> if the managed object was removed; otherwise, <c>false</c>.</returns>
    public bool Remove(ManagedObject @object)
    {
      return Remove(@object as TObject);
    }

    /// <summary>
    /// Returns a value indicating whether the specified managed object is contained in the <see cref="IManager{TObject}"/>.
    /// </summary>
    /// <param name="object">The managed object to find.</param>
    /// <returns><c>true</c> if the object was found; otherwise, <c>false</c>.</returns>
    public bool Contains(TObject @object)
    {
      return _objects.ContainsKey(@object.Guid);
    }

    /// <summary>
    /// Returns a value indicating whether the specified managed object is contained in the <see cref="IManager"/>.
    /// </summary>
    /// <param name="object">The managed object to find.</param>
    /// <returns><c>true</c> if the managed object was found; otherwise, <c>false</c>.</returns>
    public bool Contains(ManagedObject @object)
    {
      return Contains(@object as TObject);
    }


    /// <summary>
    /// Clears the <see cref="IManager"/> of all managed objects.
    /// </summary>
    public void Clear()
    {
      _objects.Clear();
    }


    /// <summary>
    /// Finds a managed object with the specified identifier.
    /// </summary>
    /// <param name="objectGuid">The identifier of the managed object to find.</param>
    /// <param name="object">The found managed object.</param>
    /// <returns><c>true</c> if the object was found; otherwise, <c>false</c>.</returns>
    public bool TryGetObject(Guid objectGuid, out IObject @object)
    {
      if (!_objects.TryGetValue(objectGuid, out var o))
      {
        @object = ManagedObject.Null;
        return false;
      }

      @object = o;
      return true;
    }


    public IEnumerator<TObject> GetEnumerator()
    {
      return _objects.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

}
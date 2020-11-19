using System;
using System.Collections.Generic;

using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.API
{

  /// <summary>
  /// Defines functionality for a storage container for managed objects.
  /// </summary>
  public interface IStore
  {
    /// <summary>
    /// Gets a managed object with the specified identifier.
    /// </summary>
    /// <param name="objectGuid">The identifier of the object to get.</param>
    /// <returns>The managed object, if found; otherwise, <c>null</c>.</returns>
    IObject this[Guid objectGuid] { get; }


    /// <summary>
    /// Gets the community which uses the current store.
    /// </summary>
    Community Community { get; }


    /// <summary>
    /// Creates a new managed object using the specified builder function.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <typeparam name="TBuilder">The builder type.</typeparam>
    /// <param name="builderFunc">The builder function to use.</param>
    /// <returns>A reference to the created object.</returns>
    TObject CreateObject<TObject, TBuilder>([NotNull] Action<TBuilder> builderFunc)
      where TObject : ManagedObject, new()
      where TBuilder : IBuilder<TObject>;

    /// <summary>
    /// Creates a new managed object using the specified arguments.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>A reference to the created object.</returns>
    TObject CreateObject<TObject>(params object[] args)
      where TObject : ManagedObject, new();


    /// <summary>
    /// Destroys the specified managed object.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <param name="object">The managed object to destroy.</param>
    void DestroyObject<TObject>(TObject @object) where TObject : ManagedObject, new();

    /// <summary>
    /// Destroys a managed object with the specified unique identifier.
    /// </summary>
    /// <param name="objectGuid">The unique identifier of the managed object to destroy.</param>
    void DestroyObject<TObject>(Guid objectGuid) where TObject : ManagedObject, new();


    /// <summary>
    /// Returns a value indicating whether the specified managed object exists.
    /// </summary>
    /// <param name="object">The managed object to find.</param>
    /// <returns><c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
    bool HasObject(IObject @object);

    /// <summary>
    /// Returns a value indicating whether a managed object with the specified identifier exists.
    /// </summary>
    /// <param name="objectGuid">The identifier of the managed object to find.</param>
    /// <returns><c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
    bool HasObject(Guid objectGuid);


    /// <summary>
    /// Returns a managed object with the specified identifier.
    /// </summary>
    /// <param name="objectGuid">The identifier of the managed object to get.</param>
    /// <returns>The object, if it exists; otherwise, <c>null</c>.</returns>
    IObject GetObject(Guid objectGuid);

    /// <summary>
    /// Returns a managed object of type <see cref="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <param name="objectGuid">The identifier of the object to get.</param>
    /// <returns>The object, if it exists; otherwise, <c>null</c>.</returns>
    TObject GetObject<TObject>(Guid objectGuid)
      where TObject : ManagedObject, new();


    /// <summary>
    /// Returns a collection of all managed objects contained in the <see cref="IStore"/>.
    /// </summary>
    /// <returns>A collection of all managed objects.</returns>
    IEnumerable<IObject> GetObjects();

    /// <summary>
    /// Returns a collection of all managed objects of type <see cref="TObject"/> in the <see cref="IStore"/>.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <returns>A collection of all managed objects.</returns>
    IEnumerable<TObject> GetObjects<TObject>()
      where TObject : ManagedObject, new();
    IManager<TObject> GetManager<TObject>() where TObject : ManagedObject, new();
    IManager GetManager([NotNull] Type type);
  }

}
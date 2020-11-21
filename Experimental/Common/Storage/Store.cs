using System;
using System.Collections.Generic;

using Discord.WebSocket;

using Experimental.API;
using Experimental.Common.Objects;

using JetBrains.Annotations;

namespace Experimental.Common.Storage
{
  /// <summary>
  /// Represents a storage container for managed objects.
  /// </summary>
  public class Store : IStore
  {
    private readonly Dictionary<Type, IManager> _managers;
    private readonly Dictionary<Guid, IObject> _objects;

    /// <summary>
    /// Initializes a new instance of the <see cref="Store"/> class, specifying the community which uses the current store.
    /// </summary>
    /// <param name="community">The community that uses the store; can be <c>null</c>.</param>
    /// <param name="guild">The guild which the store is part of; can be <c>null</c>.</param>
    /// <remarks>
    /// A community is not required in order for a store to function; leave <paramref name="community"/> as <c>null</c> if it isn't a community-based storage container.
    /// </remarks>
    public Store([CanBeNull] Community community = null, [CanBeNull] SocketGuild guild = null)
    {
      _managers = new Dictionary<Type, IManager>();
      _objects = new Dictionary<Guid, IObject>();

      Community = community;
      Guild = guild;
    }


    /// <summary>
    /// Gets a managed object with the specified identifier.
    /// </summary>
    /// <param name="objectGuid">The identifier of the object to get.</param>
    /// <returns>The managed object, if found; otherwise, <c>null</c>.</returns>
    public IObject this[Guid objectGuid] => GetObject(objectGuid);


    /// <summary>
    /// Gets the community which uses the current store.
    /// </summary>
    /// <remarks>
    /// This is <c>null</c> if this store doesn't belong to a <see cref="Objects.Community"/>.
    /// </remarks>
    public Community Community { get; }

    /// <summary>
    /// Gets the guild which the <see cref="Store"/> belongs to.
    /// </summary>
    public SocketGuild Guild { get; internal set; }


    /// <summary>
    /// Creates a new managed object using the specified builder function.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <typeparam name="TBuilder">The builder type.</typeparam>
    /// <param name="builderFunc">The builder function to use.</param>
    /// <returns>A reference to the created object.</returns>
    public TObject CreateObject<TObject, TBuilder>(Action<TBuilder> builderFunc)
      where TObject : ManagedObject, new()
      where TBuilder : IBuilder<TObject>
    {
      var builder = Activator.CreateInstance<TBuilder>();
      builderFunc(builder);

      var @object = builder.Build();
      InitializeObject(@object);

      return @object;
    }

    /// <summary>
    /// Creates a new managed object using the specified arguments.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>A reference to the created object.</returns>
    public TObject CreateObject<TObject>(params object[] args)
      where TObject : ManagedObject, new()
    {
      var @object = (TObject)Activator.CreateInstance(typeof(TObject), args);
      InitializeObject(@object);

      return @object;
    }

    /// <summary>
    /// Creates a new managed object of the specified type using the specified arguments.
    /// </summary>
    /// <param name="type">The managed object type to use.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>A reference to the created object.</returns>
    public ManagedObject CreateObject([NotNull] Type type, params object[] args)
    {
      var @object = (ManagedObject) Activator.CreateInstance(type, args);
      InitializeObject(@object);

      return @object;
    }


    /// <summary>
    /// Destroys the specified managed object.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <param name="object">The managed object to destroy.</param>
    public async void DestroyObject<TObject>(TObject @object)
      where TObject : ManagedObject, new()
    {
      if (!HasObject(@object))
      {
        return;
      }

      await @object.DestroyAsync();

      _objects.Remove(@object.Guid);
      GetManager<TObject>().Remove(@object);
    }

    /// <summary>
    /// Destroys a managed object with the specified unique identifier.
    /// </summary>
    /// <param name="objectGuid">The unique identifier of the managed object to destroy.</param>
    public void DestroyObject<TObject>(Guid objectGuid) where TObject : ManagedObject, new()
    {
      if (!HasObject(objectGuid))
      {
        return;
      }

      var manager = GetManager<TObject>() as IManager<TObject>;
      var @object = manager[objectGuid];

      _objects.Remove(objectGuid);
      manager.Remove(@object);
    }


    /// <summary>
    /// Returns a value indicating whether the specified managed object exists.
    /// </summary>
    /// <param name="object">The managed object to find.</param>
    /// <returns><c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
    public bool HasObject(IObject @object)
    {
      return HasObject(@object.Guid);
    }

    /// <summary>
    /// Returns a value indicating whether a managed object with the specified identifier exists.
    /// </summary>
    /// <param name="objectGuid">The identifier of the managed object to find.</param>
    /// <returns><c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
    public bool HasObject(Guid objectGuid)
    {
      return _objects.ContainsKey(objectGuid);
    }


    /// <summary>
    /// Returns a managed object with the specified identifier.
    /// </summary>
    /// <param name="objectGuid">The identifier of the managed object to get.</param>
    /// <returns>The object, if it exists; otherwise, <c>null</c>.</returns>
    public IObject GetObject(Guid objectGuid)
    {
      return _objects.TryGetValue(objectGuid, out var @object) ? @object : null;
    }

    /// <summary>
    /// Returns a managed object of type <see cref="TObject"/>.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <param name="objectGuid">The identifier of the object to get.</param>
    /// <returns>The object, if it exists; otherwise, <c>null</c>.</returns>
    public TObject GetObject<TObject>(Guid objectGuid)
      where TObject : ManagedObject, new()
    {
      return (TObject)(GetManager<TObject>().TryGetObject(objectGuid, out var @object) ? @object : (TObject)ManagedObject.Null);
    }


    /// <summary>
    /// Returns a collection of all managed objects contained in the <see cref="Store"/>.
    /// </summary>
    /// <returns>A collection of all managed objects.</returns>
    public IEnumerable<IObject> GetObjects()
    {
      return _objects.Values;
    }

    /// <summary>
    /// Returns a collection of all managed objects of type <see cref="TObject"/> in the <see cref="Store"/>.
    /// </summary>
    /// <typeparam name="TObject">The managed object type.</typeparam>
    /// <returns>A collection of all managed objects.</returns>
    public IEnumerable<TObject> GetObjects<TObject>()
      where TObject : ManagedObject, new()
    {
      if (!_managers.TryGetValue(typeof(TObject), out var manager))
      {
        manager = new Manager<TObject>(this);
        _managers.Add(typeof(TObject), manager);
      }

      return (IEnumerable<TObject>)manager;
    }


    private async void InitializeObject<TObject>(TObject @object)
      where TObject : ManagedObject, new()
    {
      var manager = GetManager<TObject>();

      @object.Guid = Guid.NewGuid();
      @object.Store = this;
      @object.Guild = Guild;

      _objects.Add(@object.Guid, @object);
      manager.Add(@object);

      await @object.InitializeAsync();
    }

    public IManager GetManager(Type type)
    {
      if (!_managers.TryGetValue(type, out var manager))
      {
        manager = (IManager)Activator.CreateInstance(typeof(Manager<>).MakeGenericType(type), this);
        _managers.Add(type, manager);

        Console.WriteLine($"Registered manager for managed object type `{type.FullName}`.");
      }

      return manager;
    }

    public IManager<TObject> GetManager<TObject>()
      where TObject : ManagedObject, new() =>
      (IManager<TObject>)GetManager(typeof(TObject));

  }

}
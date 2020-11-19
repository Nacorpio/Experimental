using System;
using System.Threading.Tasks;

using Discord;

using Experimental.API;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Experimental.Common.Objects
{

  /// <summary>
  /// Represents an object whose lifetime is managed.
  /// </summary>
  public class ManagedObject : IObject
  {
    /// <summary>
    /// Gets a new <see cref="ManagedObject"/> instance with a null state.
    /// </summary>
    [JsonIgnore]
    public static IObject Null => new ManagedObject();


    /// <summary>
    /// Initializes a new instance of the <see cref="ManagedObject"/> class, specifying its unique identifier.
    /// </summary>
    /// <param name="guid">The unique identifier.</param>
    internal ManagedObject(Guid guid)
    {
      Guid = guid;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagedObject"/> class.
    /// </summary>
    protected ManagedObject()
    {
      Guid = Guid.Empty;

      Guild = null;
      Store = null;
    }


    /// <summary>
    /// Gets the identifier of the managed object.
    /// </summary>
    [JsonIgnore]
    public Guid Guid { get; internal set; }


    /// <summary>
    /// Gets the store in which the managed object is stored.
    /// </summary>
    [JsonIgnore]
    public IStore Store { get; internal set; }

    /// <summary>
    /// Gets the guild in which the managed object belongs.
    /// </summary>
    [JsonProperty("guild")]
    public IGuild Guild { get; internal set; }



    /// <summary>
    /// Gets a value indicating whether the state of the managed object is null.
    /// </summary>
    [JsonIgnore]
    public bool IsNull => Guid == Guid.Empty;


    /// <summary>
    /// Initializes the managed object.
    /// </summary>
    /// <returns>A reference to a task executing this method.</returns>
    public async virtual Task InitializeAsync()
    {
    }

    /// <summary>
    /// Destroys the managed object.
    /// </summary>
    /// <returns>A reference to a task executing this method.</returns>
    public async virtual Task DestroyAsync()
    {
    }


    /// <summary>
    /// Serializes the object to a JSON object.
    /// </summary>
    /// <returns>A reference to the new JSON object.</returns>
    public virtual JObject ToJson()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Populates the current instance with the specified JSON object.
    /// </summary>
    /// <param name="json">The JSON object to populate with.</param>
    public virtual void FromJson(JObject json)
    {
      throw new NotImplementedException();
    }
  }

}
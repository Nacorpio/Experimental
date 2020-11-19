using Newtonsoft.Json.Linq;

namespace Experimental.API
{

  /// <summary>
  /// Defines functionality for an object that can be read/written in JSON.
  /// </summary>
  public interface IJsonable
  {
    /// <summary>
    /// Populates the current instance with the specified JSON object.
    /// </summary>
    /// <param name="json">The JSON object to populate with.</param>
    void FromJson(JObject json);

    /// <summary>
    /// Serializes the object to a JSON object.
    /// </summary>
    /// <returns>A reference to the new JSON object.</returns>
    JObject ToJson();
  }

}
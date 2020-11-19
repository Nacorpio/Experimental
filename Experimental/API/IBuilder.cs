using System.Threading.Tasks;

namespace Experimental.API
{
  /// <summary>
  /// Defines functionality for a builder of managed objects.
  /// </summary>
  public interface IBuilder
  {
  }

  /// <summary>
  /// Defines functionality for a builder of managed objects of type <see cref="TObject"/>.
  /// </summary>
  /// <typeparam name="TObject">The object type.</typeparam>
  public interface IBuilder<out TObject> : IBuilder
  {
    /// <summary>
    /// Builds the object and returns the result.
    /// </summary>
    /// <returns>A reference to the built object.</returns>
    TObject Build();
  }

  /// <summary>
  /// Defines functionality for a asynchronous builder of managed objects of type <see cref="TObject"/>.
  /// </summary>
  /// <typeparam name="TObject"></typeparam>
  public interface IAsyncBuilder <TObject> : IBuilder
  {
    /// <summary>
    /// Builds the object asynchronously and returns the result.
    /// </summary>
    /// <returns>A reference to the built object.</returns>
    Task<TObject> Build();
  }

  public abstract class BuilderBase <TObject> : IBuilder <TObject>
  {
    /// <summary>
    /// Builds the object and returns the result.
    /// </summary>
    /// <returns>A reference to the built object.</returns>
    public abstract TObject Build();
  }

}
using System.Threading.Tasks;

using Experimental.API;

namespace Experimental.Common.Builders
{

  public abstract class AsyncBuilderBase <TObject> : IAsyncBuilder <TObject>
  {
    /// <summary>
    /// Builds the object asynchronously and returns the result.
    /// </summary>
    /// <returns>A reference to the built object.</returns>
    public abstract Task <TObject> Build();
  }

}
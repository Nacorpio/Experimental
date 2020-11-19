using System.Text;

namespace Experimental.Common.Utilities
{

  public static class StringUtils
  {
    /// <summary>
    /// Separates each value in the specified array by a comma.
    /// </summary>
    /// <param name="values">The values to separate.</param>
    /// <returns>A reference to the new <see cref="string"/>.</returns>
    public static string Separate(params string[] values)
    {
      return string.Join(",", values);
    }

    /// <summary>
    /// Builds a <see cref="string"/> consisting of multiple lines.
    /// </summary>
    /// <param name="lines">The lines of the string.</param>
    /// <returns>A reference to the new <see cref="string"/>.</returns>
    public static string Lines(params string[] lines)
    {
      var sb = new StringBuilder();

      foreach (var line in lines)
      {
        sb.AppendLine(line);
      }

      return sb.ToString();
    }
  }

}
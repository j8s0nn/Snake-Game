// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//       A class represents a point in 2D coordinates.
//   </para>
// </summary>


namespace GUI.Components.Models;

/// <summary>
/// Represents a point in 2D space using integer coordinates.
/// </summary>
public class Point2D
{
    /// <summary>
    /// Gets or sets the X-coordinate of the point.
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// Gets or sets the Y-coordinate of the point.
    /// </summary>
    public int Y { get; set; }

    
    /// <summary>
    /// Initialize a point with specific X and Y coordinates
    /// </summary>
    /// <param name="x">The X-coordinates of the point</param>
    /// <param name="y">The Y-coordinates of the point</param>
    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initialize a point with default coordinates of (0,0)
    /// </summary>
    public Point2D()
    {
        X = 0;
        Y = 0;
    }
    
}
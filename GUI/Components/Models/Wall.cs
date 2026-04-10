// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//       Represents a wall in the snake game
//   </para>
// </summary>


using System.Text.Json.Serialization;

namespace GUI.Components.Models;

public class Wall
{
    /// <summary>
    /// Gets or sets the unique identifier of the wall.
    /// </summary>
    [JsonPropertyName("wall")] public int ID { get; set; } 

    /// <summary>
    /// Gets or sets the first endpoint of the wall.
    /// </summary>
    [JsonPropertyName("p1")] public Point2D p1 { get; set; }
    
    /// <summary>
    /// Gets or sets the first endpoint of the wall.
    /// </summary>
    [JsonPropertyName("p2")] public Point2D p2 { get; set; }

    
    /// <summary>
    /// Initialize a default wall with ID = 0.
    /// </summary>
    public Wall()
    {
        ID = 0; 
        p1 = new Point2D();
        p2 = new Point2D();
    }
    
    
}
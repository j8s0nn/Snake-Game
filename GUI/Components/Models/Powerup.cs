// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//       Represents powerup (collectible items) in the snake game
//   </para>
// </summary>


using System.Text.Json.Serialization;

namespace GUI.Components.Models;

/// <summary>
/// Represents a collectible power-up in the game world.
/// Power-ups appear at a specific location and can be consumed by players.
/// </summary>
public class Powerup
{
    /// <summary>
    /// Gets or sets the unique identifier of the power-up.
    /// </summary>
    [JsonPropertyName("power")] public int ID { get; set; }
    
    /// <summary>
    /// Gets or sets the location of the power-up in the game world.
    /// </summary>
    [JsonPropertyName("loc")] public Point2D Location { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether the power-up has been consumed
    /// or is no longer active in the world.
    /// </summary>
    [JsonPropertyName("died")] public bool IsDied { get; set; }

    /// <summary>
    /// Initialize a default powerup object with ID = 0
    /// </summary>
    public Powerup()
    {
        ID = 0;
        Location = new Point2D();
        IsDied = false;
    }
    
}
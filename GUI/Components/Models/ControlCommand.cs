// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//     Represents a control command used to specify player movement in the snake game.
//   </para>
// </summary>


using System.Text.Json.Serialization;

namespace GUI.Components.Models;


/// <summary>
/// Represents a control command sent from the client to the server,
/// indicating the desired movement direction of the player.
/// </summary>
public class ControlCommand
{
    
    /// <summary>
    /// Gets or sets the movement direction command.Possible values include "up", "down", "left", "right", or "none".
    /// </summary>
    [JsonPropertyName("moving")] public string Moving { get; set; }

    /// <summary>
    /// Initializes a new movement with no movement ("none").
    /// </summary>
    public ControlCommand()
    {
        Moving = "none";
    }
    
}
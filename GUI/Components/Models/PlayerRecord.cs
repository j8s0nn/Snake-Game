// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/21/2026 </date>
//        Represents a record of a player’s performance and participation in a game,
//        including identity, score, and time information.
//   </para>
// </summary>

namespace GUI.Components.Models;


/// <summary>
/// Represents a database record for a single player within a specific game session.
/// </summary>
public class PlayerRecord
{
    
    /// <summary>
    /// Gets or sets the player ID sent by the server
    /// </summary>
    public int ID { get; set; }

    
    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the highest score achieved by the player in the game.
    /// </summary>
    public int MaxScore { get; set; }
    
    /// <summary>
    /// Gets or sets the time the player joined the game.
    /// </summary>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// Gets or sets the time the player left the game.
    /// </summary>
    /// <value>
    /// This value is null if the player is still active in the game.
    /// </value>
    public DateTime? EndTime { get; set; }
    
    
}
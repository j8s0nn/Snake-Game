// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/21/2026 </date>
//       Handles creating and updating game records, tracking player state changes,
//       and retrieving stored game statistics from the database.
//   </para>
// </summary>


namespace GUI.Components.Models;


/// <summary>
/// Represents a record of a single game session stored in the database.
/// </summary>
/// <remarks>
/// Each game record contains the unique identifier of the game,
/// along with its start time and optional end time.
/// The EndTime will be null if the game is still in progress
/// or has not been properly terminated.
/// </remarks>
public class GameRecord
{
    
    /// <summary>
    /// Gets or sets the GameID which is auto-incremented by the database
    /// </summary>
    public int GameId { get; set; }
    
    /// <summary>
    /// Gets or sets the time at which the game started.
    /// </summary>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// Gets or sets the time at which the game ended.
    /// </summary>
    /// <value>
    /// This value is null if the game is still ongoing or has no recorded end time.
    /// </value>
    public DateTime? EndTime { get; set; }
}
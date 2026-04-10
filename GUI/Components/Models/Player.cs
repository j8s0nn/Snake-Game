// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//       Represents a snake in the snake game
//   </para>
// </summary>



using System.Text.Json.Serialization;

namespace GUI.Components.Models;

/// <summary>
/// Represents a player (snake) in the game. A player contains identifying information,
/// body segments, movement direction, score, and state flags such as alive or dead.
/// </summary>
public class Player
{
    /// <summary>
    /// Gets or sets the unique identifier of the player.
    /// </summary>
    [JsonPropertyName("snake")] public int ID { get; set; }

    
    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    [JsonPropertyName("name")] public string Name { get; set; }

    
    /// <summary>
    /// Gets or sets the list of points representing the snake's body segments.
    /// The first element is the tail, and the last element is the head.
    /// </summary>
    [JsonPropertyName("body")] public List<Point2D> Body { get; set; }

    
    /// <summary>
    /// Gets or sets the current direction of movement of the snake.
    /// </summary>
    [JsonPropertyName("dir")] public Point2D Direction { get; set; }

    
    /// <summary>
    /// Gets or sets the current score of the player.
    /// </summary>
    [JsonPropertyName("score")] public int Score { get; set; }

    
    /// <summary>
    /// Gets or sets the current score of the player.
    /// </summary>
    [JsonPropertyName("died")] public bool Died { get; set; } = false;

    
    /// <summary>
    /// Gets or sets a value indicating whether the player is currently alive.
    /// </summary>
    [JsonPropertyName("alive")] public bool Alive { get; set; } = true;

    
    /// <summary>
    /// Gets or sets a value indicating whether the player has disconnected.
    /// </summary>
    [JsonPropertyName("dc")] public bool IsDisconnected { get; set; }

    
    /// <summary>
    /// Gets or sets a value indicating whether the player has joined the game.
    /// </summary>
    [JsonPropertyName("join")] public bool Joined { get; set; }

    
    /// <summary>
    /// Gets or sets a value used on the client side to track whether the player
    /// was previously dead. This was used for drawing explosions when the player died.
    /// </summary>
    public bool WasDead { get; set; } = false;


    /// <summary>
    /// Initialize a default player
    /// </summary>
    public Player()
    {
        ID = -1;
        Name = string.Empty;
        Body = new List<Point2D>();
        Direction = new Point2D();
        Score = 0;
        IsDisconnected = false;
        Joined = true;
    }


    /// <summary>
    /// Initialize a new player as a deep copy of another player
    /// </summary>
    /// <param name="other">The player to copy from</param>
    public Player(Player other)
    {
        ID = other.ID;
        Name = other.Name;
        Score = other.Score;
        IsDisconnected = other.IsDisconnected;
        Joined = other.Joined;
        Died = other.Died;
        Alive = other.Alive;
        Body = new List<Point2D>(other.Body);
        Direction = new Point2D(other.Direction.X, other.Direction.Y);
    }
    

}
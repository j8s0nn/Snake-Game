using System.Text.Json.Serialization;

namespace GUI.Components.Models;

//TODO:Documents
public class Player
{
    [JsonPropertyName("snake")] public int ID { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("body")] public List<Point2D> Body { get; set; }

    [JsonPropertyName("dir")] public Point2D Direction { get; set; }

    [JsonPropertyName("score")] public int Score { get; set; }

    [JsonPropertyName("died")] public bool Died { get; set; } = false;

    [JsonPropertyName("alive")] public bool Alive { get; set; } = true;

    [JsonPropertyName("dc")] public bool IsDisconnected { get; set; }

    [JsonPropertyName("join")] public bool Joined { get; set; }

    public bool WasDead { get; set; } = false;


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


    public Player(Player other)
    {
        ID = other.ID;
        Name = other.Name;
        Score = other.Score;
        IsDisconnected = other.IsDisconnected;
        Joined = other.Joined;
        Died = other.Died;
        Alive = other.Alive;
        
        //Make a copy
        Body = new List<Point2D>(other.Body);
        
        //Make a copy 
        Direction = new Point2D(other.Direction.X, other.Direction.Y);
        
    }
    

}
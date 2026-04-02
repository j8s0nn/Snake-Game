using System.Text.Json.Serialization;

namespace GUI.Components.Models;

public class Powerup
{
    [JsonPropertyName("power")] public int ID { get; set; }
    
    [JsonPropertyName("loc")] public Point2D Location { get; set; }

    [JsonPropertyName("died")] public bool IsDied { get; set; }

    public Powerup()
    {
        ID = 0;
        Location = new Point2D();
        IsDied = false;
    }

    public Powerup(Powerup other)
    {
        ID = other.ID;
        Location = new Point2D(other.Location.X, other.Location.Y);
        IsDied = other.IsDied;
    }
}
using System.Drawing;
using System.Text.Json.Serialization;

namespace GUI.Components.Models;

public class Wall
{
    [JsonPropertyName("wall")] public int ID { get; set; } // Must be unique

    
    [JsonPropertyName("p1")] public Point2D p1 { get; set; }
    
    
    [JsonPropertyName("p2")] public Point2D p2 { get; set; }

    public Wall()
    {
        ID = 0;
        p1 = new Point2D();
        p2 = new Point2D();
    }


    public Wall(Wall other)
    {
        ID = other.ID;
        p1 = new Point2D(other.p1.X, other.p1.Y);
        p2 = new Point2D(other.p2.X, other.p2.Y);
    }
}
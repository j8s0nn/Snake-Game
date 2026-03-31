using System.Text.Json.Serialization;

namespace GUI.Components.Models;

//TODO:Documents
public class Player
{
    [JsonPropertyName("snake")] public int ID { get; set; }
    
    [JsonPropertyName("name")] public int Name { get; set; }

    [JsonPropertyName("body")] public List<Point2D>  Body { get; set; }
    
    [JsonPropertyName("dir")] public Point2D Dir { get; set; }

    [JsonPropertyName("score")] public int Score { get; set; }

    [JsonPropertyName("died")] public bool Died { get; set; } = false;

    [JsonPropertyName("alive")] public bool Alive { get; set; } = true;

    [JsonPropertyName("dc")] public bool IsDisconnected { get; set; }

    [JsonPropertyName("join")] public bool Joined { get; set; }
}
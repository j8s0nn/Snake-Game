using System.Text.Json.Serialization;

namespace GUI.Components.Models;

public class ControlCommand
{
    [JsonPropertyName("moving")] public string Moving { get; set; }
}
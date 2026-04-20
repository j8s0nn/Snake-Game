namespace GUI.Components.Models;

public class GameRecord
{
    public int GameId { get; set; }
    
    public DateTime StartTime { get; set; }
    
    //Can be null
    public DateTime? EndTime { get; set; }
}
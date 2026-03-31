namespace GUI.Components.Models;

public class World
{
    public Dictionary<int, Player> Players { get; private set; }

    public Dictionary<int, Wall> Walls { get; private set; }
    
    public Dictionary<int, Powerup> Powerups { get; private set; }

    private int Width { get; set; }
    private int Height { get; set; }

    // public World(int width, int height)
    // {
    //     
    // }
    

    public void SetPlayerID(int ID)
    {
        if (Players.ContainsKey(ID))
        {
            Players[ID] = new Player();
        }
        else
        {
            Players.Add(ID, new Player());
        }
    }



    public void SetWorldSize(int width, int height)
    {
        Width = width;
        Height = height;
    }


}
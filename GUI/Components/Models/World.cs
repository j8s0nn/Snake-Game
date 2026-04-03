namespace GUI.Components.Models;

public class World
{
    public Dictionary<int, Player> Players { get; set; }

    
    public Dictionary<int, Wall> Walls { get; set; }
    
    public Dictionary<int, Powerup> Powerups { get; set; }

    public int playerID { get; set; }

    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;

    public bool HasChanged { get; set; } = false;

    public World()
    {
        Players = new Dictionary<int, Player>();
        Walls = new Dictionary<int, Wall>();
        Powerups = new Dictionary<int, Powerup>();
        HasChanged = false;
    }

    
    /// <summary>
    /// This constructor is used for creating a copy of a world
    /// </summary>
    /// <param name="oldWorld">the old world that needs to make a copy of</param>
    public World(World oldWorld)
    {
        Players = new Dictionary<int, Player>(oldWorld.Players);
        Walls = new Dictionary<int, Wall>(oldWorld.Walls);
        Powerups = new Dictionary<int, Powerup>(oldWorld.Powerups);
        Width = oldWorld.Width;
        Height = oldWorld.Height;
        HasChanged = oldWorld.HasChanged;
        playerID = oldWorld.playerID;
    }

    public Point2D GetSnakeHead()
    {
        return this.Players[playerID].Body[^1];
    }

    // public void SetPlayerId(int ID)
    // {
    //     if (Players.ContainsKey(ID))
    //     {
    //         Players[ID] = new Player();
    //     }
    //     else
    //     {
    //         Players.Add(ID, new Player());
    //     }
    //     
    //     HasChanged = true;
    //
    // }

    public void AddPlayer(Player player)
    {
        if (player == null)
        {
            return;
        }
        Players[player.ID] = player;
        
        Console.WriteLine($"Added player | ID: {player.ID}, Name: {player.Name}, Score: {player.Score}, Alive: {player.Alive}, Died: {player.Died}, Disconnected: {player.IsDisconnected}, Joined: {player.Joined}, Body Length: {player.Body?.Count ?? 0}");
        HasChanged = true;
    }

        
        
        
        //TODO: Debug
        // foreach (var p in Players.Values)
        // {
        //     Console.WriteLine("");
        // }
    


    public void SetSize(int width, int height)
    {
        
        Width = width;
        Height = height;
            
        HasChanged = true;

        //TODO: Debug
        
        Console.WriteLine($"Width: {Width}, Height: {Height}");
    }

    public void AddWalls(Wall wall)
    {
        if (wall == null)
        {
            return;
        }

        Walls[wall.ID] = wall;
        
            
        Console.WriteLine($"Added wall {wall.ID} | p1: ({wall.p1.X}, {wall.p1.Y}) | p2: ({wall.p2.X}, {wall.p2.Y})");
        
            
        HasChanged = true;


    }

    public void AddPowerup(Powerup powerup)
    {
        if (powerup == null)
        {
            return;
        }

        Powerups[powerup.ID] = powerup;
        
        // if (Powerups.ContainsKey(powerup.ID))
        // {
        //     
        //     Console.WriteLine($"Added Power | ID: {powerup.ID}, Location: ({powerup.Location.X}, {powerup.Location.Y}), Died: {powerup.IsDied}");
        // }
        // else
        // {
        //     Powerups.Add(powerup.ID, powerup);
        //     
        //     Console.WriteLine($"Added Power | ID: {powerup.ID}, Location: ({powerup.Location.X}, {powerup.Location.Y}), Died: {powerup.IsDied}");
        // }
        
        HasChanged = true;
    }




}
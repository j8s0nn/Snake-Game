namespace GUI.Components.Models;

public class World
{
    public Dictionary<int, Player> Players { get; private set; }

    
    public Dictionary<int, Wall> Walls { get; private set; }
    
    public Dictionary<int, Powerup> Powerups { get; private set; }

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
        Players = new Dictionary<int, Player>();
        foreach (var oldPlayer in oldWorld.Players)
        {
            Players[oldPlayer.Key] = new Player(oldPlayer.Value);
        }

        Walls = new Dictionary<int, Wall>();
        foreach (var oldWall in oldWorld.Walls)
        {
            Walls[oldWall.Key] = new Wall(oldWall.Value);
        }

        Powerups = new Dictionary<int, Powerup>();
        foreach (var oldPowerup in oldWorld.Powerups)
        {
            Powerups[oldPowerup.Key] = new Powerup(oldPowerup.Value);
        }

        Width = oldWorld.Width;
        Height = oldWorld.Height;
        HasChanged = oldWorld.HasChanged;
    }


    public void SetPlayerId(int ID)
    {
        if (Players.ContainsKey(ID))
        {
            Players[ID] = new Player();
        }
        else
        {
            Players.Add(ID, new Player());
        }
        
        HasChanged = true;
    
    }

    public void AddPlayer(Player player)
    {
        if (player == null)
        {
            return;
        }
        
        if (Players.ContainsKey(player.ID))
        {
            Players[player.ID] = player;
        }
        else
        {
            Players.Add(player.ID, player);
        }

        
        HasChanged = true;
    }


    public void SetSize(int width, int height)
    {
        
        Width = width;
        Height = height;
            
        HasChanged = true;

    }

    public void AddWalls(Wall wall)
    {
        if (wall == null)
        {
            return;
        }

        if (Walls.ContainsKey(wall.ID))
        {
            Walls[wall.ID] = wall;
        }
        else
        {
            Walls.Add(wall.ID, wall);
        }
            
        HasChanged = true;


    }

    public void AddPowerup(Powerup powerup)
    {
        if (powerup == null)
        {
            return;
        }

        if (Powerups.ContainsKey(powerup.ID))
        {
            Powerups[powerup.ID] = powerup;
        }
        else
        {
            Powerups.Add(powerup.ID, powerup);
        }


        HasChanged = true;
    }




}
// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//       Represents the world (including players, powerup, and walls) in the snake game
//   </para>
// </summary>


using GUI.Components.Controllers;

namespace GUI.Components.Models;

using System;


/// <summary>
/// Represents the complete state of the game world.
/// It contains all entities such as players, walls, power-ups,
/// and explosion effects used for rendering.
/// </summary>
public class World
{
    
    /// <summary>
    /// Gets or sets the collection of players in the world,
    /// indexed by their unique ID.
    /// </summary>
    public Dictionary<int, Player> Players { get; set; }

    /// <summary>
    /// Gets or sets the collection of walls in the world,
    /// indexed by their unique ID.
    /// </summary>
    public Dictionary<int, Wall> Walls { get; set; }
    
    
    /// <summary>
    /// Gets or sets the collection of power-ups in the world,
    /// indexed by their unique ID.
    /// </summary>
    public Dictionary<int, Powerup> Powerups { get; set; }
    
    
    /// <summary>
    /// Gets or sets the collection of death positions,
    /// indexed by player ID. These are used to render explosion effects.
    /// </summary>
    public Dictionary<int, Point2D> DeathPositions { get; set; }

    public bool HasRemoved { get; set; } =  false;
    
    public bool HasAdded { get; set; } =  false;

    /// <summary>
    /// Gets or sets the ID of the current player.
    /// </summary>
    public int playerID { get; set; }

    /// <summary>
    /// Gets or sets the width of the world.
    /// </summary>
    public int Width { get; set; } = 0;
    
    /// <summary>
    /// Gets or sets the height of the world.
    /// </summary>
    public int Height { get; set; } = 0;


    private DatabaseController database;
    
    /// <summary>
    /// Initialize a default world.
    /// </summary>
    public World()
    {
        Players = new Dictionary<int, Player>();
        Walls = new Dictionary<int, Wall>();
        Powerups = new Dictionary<int, Powerup>();
        DeathPositions = new Dictionary<int, Point2D>();
    }

    
    /// <summary>
    /// Initialize a world object as a copy of an existing world
    /// </summary>
    /// <param name="oldWorld">the old world that needs to make a copy of</param>
    public World(World oldWorld)
    {
        Players = new Dictionary<int, Player>(oldWorld.Players);
        Walls = new Dictionary<int, Wall>(oldWorld.Walls);
        Powerups = new Dictionary<int, Powerup>(oldWorld.Powerups);
        DeathPositions = new Dictionary<int, Point2D>(oldWorld.DeathPositions);
        Width = oldWorld.Width;
        Height = oldWorld.Height;
        playerID = oldWorld.playerID;
    }

    /// <summary>
    /// Get the position of a player's head.
    /// </summary>
    /// <returns></returns>
    public Point2D GetSnakeHead()
    {
        return this.Players[playerID].Body[^1];
    }
    
    /// <summary>
    /// Adds or updates a player in the world.
    /// </summary>
    /// <param name="player">The player to add or update.</param>
    public void AddPlayer(Player player)
    {
        if (player == null)
        {
            return;
        }

        lock (Players)
        {
            if (Players.TryGetValue(player.ID, out Player existingPlayer))
            {
                // Preserve the MaxScore we are tracking
                player.MaxScore = Math.Max(existingPlayer.MaxScore, player.Score);
            
                // Preserve the EnterTime
                player.EnterTime = existingPlayer.EnterTime;
            
                // Now update the reference
                Players[player.ID] = player;
            }
            else
            {
                
                player.EnterTime = DateTime.Now;
                player.MaxScore = player.Score;
                Players[player.ID] = player;
            }
        }

        
        

        
        
    }

    /// <summary>
    /// Removes the current player from the world.
    /// </summary>
    public void RemovePlayer()
    {

        lock (Players)
        {
            
            Players[playerID].LeaveTime = DateTime.Now;
            
            Players.Remove(playerID);
        }

    }


    /// <summary>
    /// Sets the size of the world.
    /// </summary>
    /// <param name="width">The width of the world.</param>
    /// <param name="height">The height of the world.</param>
    public void SetSize(int width, int height)
    {
        
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Adds or updates a wall in the world.
    /// </summary>
    /// <param name="wall">The wall to add or update.</param>
    public void AddWalls(Wall wall)
    {
        if (wall == null)
        {
            return;
        }

        Walls[wall.ID] = wall;
        
    }
    
    /// <summary>
    /// Adds or updates a power-up in the world.
    /// </summary>
    /// <param name="powerup">The power-up to add or update.</param>
    public void AddPowerup(Powerup powerup)
    {

        

        if (powerup == null)
        {
            return;
        }
        lock (Powerups)
        {
            Powerups[powerup.ID] = powerup;
            
        }
    }


    /// <summary>
    /// Adds a death position for a player to render an explosion effect.
    /// </summary>
    /// <param name="playerId">The ID of the player who died.</param>
    /// <param name="deathPosition">The position where the player died.</param>
    public void AddDeathPosition(int playerId, Point2D deathPosition)
    {
        if (deathPosition == null)
        {
            return;
        }

        DeathPositions[playerId] = deathPosition;
    }
    

}
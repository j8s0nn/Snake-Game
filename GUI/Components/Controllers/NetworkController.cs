// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/10/2026 </date>
//       Handles network communication between the client and the game server
//       for the snake game.
//   </para>
// </summary>



using GUI.Components.Models;
using System.Text.Json;

namespace GUI.Components.Controllers;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Manages the client-side network connection, including sending player input
/// and receiving game state updates from the server.
/// </summary>
/// <remarks>
/// This class is responsible for:
/// <list type="bullet">
/// <item>Establishing and maintaining a connection to the server</item>
/// <item>Parsing incoming JSON messages into game objects</item>
/// <item>Updating the <see cref="World"/> with server data</item>
/// <item>Sending movement commands based on user input</item>
/// </list>
/// </remarks>
public class NetworkController
{
     
     /// <summary>
     /// The network connection of the current player used to communicate with the server
     /// </summary>
     private NetworkConnection _client;

     /// <summary>
     /// Gets a value indicating whether the client is currently connected to the server.
     /// </summary>
     public bool IsConnected => _client != null && _client.IsConnected;

     /// <summary>
     /// Gets or sets the error message to display to the user.
     /// </summary>
     public string ErrorMessage { get; set; } = string.Empty;
     
     //TODO: privacy
     public string connectionString =
          "server=atr.eng.utah.edu;" +
          "database=u1579771;" +
          "uid=u1579771;" +
          "password=0974107730";
     
     private DateTime endTime;

     private DatabaseController databaseController;

     private int currentGameId;

     private HashSet<int> insertedPlayers;
     
     
     /// <summary>
     /// Initialize a new network connection (client) used for reading inputs from the server.
     /// </summary>
     public NetworkController()
     {
          _client = new NetworkConnection();
          databaseController = new DatabaseController(connectionString);
          insertedPlayers = new();
     }

     /// <summary>
     /// Connects to the game server and starts processing incoming data.
     /// </summary>
     /// <param name="serverAddress">The server address.</param>
     /// <param name="port">The server port.</param>
     /// <param name="name">The player name.</param>
     /// <param name="world">The game world to update.</param>
     public void Connect(string serverAddress, int port, string name, World world)
     {
          
          if (string.IsNullOrWhiteSpace(name))
          {
               ErrorMessage = "Please enter a name (Less than 16 characters)";
               return;
          }

          if (serverAddress != "localhost")
          {
               ErrorMessage = "Invalid server address";
               return;
          }

          if (port != 11000)
          {
               ErrorMessage = "Invalid port";
               return;
          }

          //Create a new connection
          _client = new NetworkConnection();

          _client.Connect(serverAddress, port);
          
          _client.Send(name);
          
          // currentGameId = databaseController.UpdateGameStartTimeClient(DateTime.Now);
          
          ProcessInput(world);
          
          
     }


     /// <summary>
     /// Continuously process the incoming data from the server
     /// </summary>
     /// <param name="world">The game world to update</param>

     private void ProcessInput(World world)
     {

          try
          {
               HandleFirstLineInput(world);
               
               while (IsConnected)
               {
                    HandleInput(world);
               }
               
          }
          catch (Exception e)
          {
               HandleError(e);
          }
     }

     /// <summary>
     /// Handles the initial data sent by the server, including player ID
     /// and world size.
     /// </summary>
     /// <param name="world">The game world to update.</param>
     private void HandleFirstLineInput(World world)
     {
          string input;
          try
          {
               
               //TODO: If client connect, then it will receive the information from the snake. If the client doesn't connect and the AI client connect and disconnect in the mean time, how could we handle
               
               //Read the ID
               input = _client.ReadLine();
               
               if (Int32.TryParse(input, out int id))
               {
                    world.playerID = id;
                    
               }
               else
               {
                    _client.Disconnect();
               }
               
               
               //Read the size of the world
               input = _client.ReadLine();

               if (Int32.TryParse(input, out int size))
               {
                    world.SetSize(size, size); // World are assumed to be square
               }
               else
               {
                    _client.Disconnect();
               }
               
               
               currentGameId = databaseController.UpdateGameStartTimeClient(DateTime.Now);
               //Update after receiving the playerID from the world
               
          }
          catch (Exception e) 
          {
               HandleError(e);
          }
     }


     /// <summary>
     /// Handles a single line of input from the server and updates the world accordingly.
     /// </summary>
     /// <param name="world">The game world to update.</param>
     private void HandleInput(World world)
     {
          
          
          try
          {
               string line = _client.ReadLine();
               
               if (line.Contains("wall") && line.Contains("p1") && line.Contains("p2"))
               {
                    Wall? wall = JsonSerializer.Deserialize<Wall>(line);
                    if (wall != null)
                    {
                         world.AddWalls(wall);
                    }


               }
               else if (line.Contains("power") && line.Contains("loc") && line.Contains("died"))
               {
                    
                    Powerup? powerup = JsonSerializer.Deserialize<Powerup>(line);
                    if (powerup != null)
                    {
                         world.AddPowerup(powerup);
                    }

               }
               else // Player
               {
                    //Write the data
                    // Console.WriteLine(line);
                    

                    Player? player = JsonSerializer.Deserialize<Player>(line);
                    
                    if (player != null)
                    {
                         
                         
                         //For update AIClient
                         if (player.IsDisconnected)
                         {
                              if (world.Players.TryGetValue(player.ID, out Player existing))
                              {
                                   player.LeaveTime = DateTime.Now;
                                   
                                   databaseController.UpdatePlayerLeaveTime(currentGameId, player);

                                   world.RemovePlayer();
                              }
                              
                              return;
                               
                         }
                         
                         if (!insertedPlayers.Contains(player.ID)) // FIRST time seeing this player
                         {
                              
                              player.EnterTime = DateTime.Now;
                              
                              databaseController.InsertPlayer(currentGameId, player);

                              insertedPlayers.Add(player.ID);
                              
                              Console.WriteLine($"Added player ID {player.ID} ");
                         }
                         
                         
                         if (world.Players.TryGetValue(player.ID, out Player existingPlayer))
                         {
                              // Console.WriteLine("Current game ID" + currentGameId + "Player game ID " + player.GameId);
                              
                              
                              if (player.Score > existingPlayer.MaxScore)
                              {
                                   player.MaxScore = player.Score;
                                   databaseController.UpdatePlayerMaxScore(currentGameId, player);
                              }
                              else
                              {
                                   player.MaxScore = existingPlayer.MaxScore;
                              }
                         }
                         
                         // Console.WriteLine(player.Name + " " + player.ID + " " + world.playerID +" " + player.MaxScore);
                         
                         //Update for explosion effect
                         if (player.Died && !player.WasDead)
                         {
                              world.AddDeathPosition(player.ID, player.Body[^1]);
                              player.WasDead = true;
                         }
                         
                         world.AddPlayer(player);
                         // Console.WriteLine("Added player " + player.ID);
                         
                         
                         
                    }
               }
          }
          catch (Exception e)
          {
               
               // For client when hit disconnect
               if (world.Players.TryGetValue(world.playerID, out Player player))
               {
                    player.LeaveTime = DateTime.Now;
                    
                    databaseController.UpdatePlayerLeaveTime(currentGameId, player);

                    world.RemovePlayer();
                    
               }
               
               
               HandleError(e);
               
               // world.RemovePlayer();
          }
     }



     /// <summary>
     /// Handles network errors by disconnecting the client.
     /// </summary>
     /// <param name="e">The exception that occurred.</param>
     private void HandleError(Exception e)
     {
          
          _client?.Disconnect();
          
          _client = null;
     }


     /// <summary>
     /// Disconnects from the server and removes the current player from the world.
     /// </summary>
     /// <param name="world">The game world</param>
     public void Disconnect(World world)
     {
          // string line = _client.ReadLine();
          
          // endTime = DateTime.Now;
          
          //Update the endtime of the game table
          world.Players[world.playerID].LeaveTime = DateTime.Now;
          databaseController.UpdateGameEndTimeClient(currentGameId, world.Players[world.playerID].LeaveTime);
          
          _client?.Disconnect();
          
          _client = null;
          
          // world.RemovePlayer();
          
          // world.Players.Clear();
     }

     /// <summary>
     /// Sends a movement command to the server based on user keyboard input.
     /// Supported keys include "W", "A", "S", "D" and the arrow keys.
     /// </summary>
     /// <param name="key">
     /// The key pressed by the user, representing a movement direction.
     /// </param>
     public void HandleMovement( string key)
     {

          if (!_client.IsConnected)
          {
               return;
          }

          if (key == "w" || key == "ArrowUp")
          {
               _client.Send("{\"moving\":\"up\"}");
          }

          if (key == "a" || key == "ArrowLeft")
          {
               _client.Send("{\"moving\":\"left\"}");
          }

          if (key == "d" || key == "ArrowRight")
          {
               _client.Send("{\"moving\":\"right\"}");
          }

          if (key == "s" || key == "ArrowDown")
          {
               _client.Send("{\"moving\":\"down\"}");
          }
          
     }
     
    
}
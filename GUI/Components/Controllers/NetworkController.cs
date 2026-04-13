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
using MySql.Data.MySqlClient;

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

     private int gameID;
     
     private DateTime startTime;
     
     private DateTime endTime;
     
     
     //TODO: privacy
     public static readonly string connectionString =
          "server=atr.eng.utah.edu;" +
          "database=u1579771;" +
          "uid=u1579771;" +
          "password=0974107730";
     
     
     /// <summary>
     /// Initialize a new network connection (client) used for reading inputs from the server.
     /// </summary>
     public NetworkController()
     {
          _client = new NetworkConnection();
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
          
          startTime = DateTime.Now;
          
          _client.Send(name);
          
          //After succesfully connect to the game, 
          UpdateStartTime();
          
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
          string line = _client.ReadLine();
          
          
          try
          {
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
                    
                    
                    Player? player = JsonSerializer.Deserialize<Player>(line);
                    
                    
                    
                    if (player != null)
                    {
                         if (player.Died && !player.WasDead)
                         {
                              world.AddDeathPosition(player.ID, player.Body[^1]);
                              player.WasDead = true;
                         }

                         world.AddPlayer(player);
                         
                    }

               }
          }
          catch (Exception e)
          {
               HandleError(e);
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
          _client?.Disconnect();
          
          endTime = DateTime.Now;


          UpdateEndTime();
          
          PrintGame();
          
          world.RemovePlayer();
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
     
     private void UpdateStartTime()
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               
               conn.Open();

               Console.WriteLine("Database connection successful!");
               
               
               MySqlCommand command = conn.CreateCommand();
               
               command.CommandText =
                    @"INSERT INTO Game (StartTime)
                      VALUES (@startTime)";
               
               command.Parameters.AddWithValue("@startTime", startTime);
               
               command.ExecuteNonQuery();
               
               //Store the ID
               command.CommandText = "SELECT LAST_INSERT_ID()";
               gameID = Convert.ToInt32(command.ExecuteScalar());
               
          }
     }

     private void UpdateEndTime()
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               
               conn.Open();

               Console.WriteLine("Database connection successful!");
               
               
               MySqlCommand command = conn.CreateCommand();
               
               //Update the existing row
               command.CommandText =
                    @"Update Game set EndTime = @endTime WHERE ID = @gameID";
               
               command.Parameters.AddWithValue("@endTime", endTime);
               command.Parameters.AddWithValue("@gameID", gameID);
               
               command.ExecuteNonQuery();
               
               
          }
               
     }

     private void PrintGame()
     {
          using ( MySqlConnection conn = new MySqlConnection( connectionString ) )
          {
               try
               {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT ID, StartTime, EndTime from Game";

                    // Execute the command and cycle through the DataReader object
                    using ( MySqlDataReader reader = command.ExecuteReader() )
                    {
                         while ( reader.Read() )
                         {
                              Console.WriteLine( reader["ID"] + " " + reader["StartTime"]  + " " + reader["EndTime"] );
                         }
                    }
               }
               catch ( Exception e )
               {
                    Console.WriteLine( e.Message );
               }
          }
     }
}
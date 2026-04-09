using System.Diagnostics;
using GUI.Components.Models;
using Microsoft.VisualBasic.CompilerServices;
using System.Text.Json;

namespace GUI.Components.Controllers;

public class NetworkController
{
     private NetworkConnection _client;

     
     public bool IsConnected => _client != null && _client.IsConnected;

     public string ErrorMessage { get; set; } = string.Empty;


     public NetworkController()
     {
          _client = new NetworkConnection();
     }

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
          
          ProcessInput(world);
     }



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
          catch (Exception e) // Disconnect when there is an error occurred when reading from server
          {
               HandleError(e);
          }
     }


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
               else if (line.Contains("power") && line.Contains("loc"))
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
                              Console.WriteLine("Added death position");
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




     private void HandleError(Exception e)
     {
          _client?.Disconnect();

          _client = null;

     }


     public void Disconnect(World world)
     {
          _client?.Disconnect();
          world.RemovePlayer();
     }


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
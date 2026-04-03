using System.Diagnostics;
using GUI.Components.Models;
using Microsoft.VisualBasic.CompilerServices;
using System.Text.Json;

namespace GUI.Components.Controllers;

public class NetworkController
{
     private NetworkConnection _client;

 

     /// <summary>
     ///Store the current Task that working on. 
     /// </summary>
     // private Task? _task;
     //TODO: Remove after. Task is like the promise, a task(a method) gonna be executed (be run and finish in the future) in the future . 
     //TODO: A task can be in background operation like calling Task.Run on the void method like handling input or processing the data. 
     //TODO: When dealing with task, be aware of `await`. Without await, when a Task start, it immediately return a Task without caring about the result of the method being executed in Task. This mean that we will no longer receive the result by the method running inside Task.
     //TODO: Task is just the placeholder, not store the actual result. To get the result, we have to attach `await` to a Task then assign it to a variable i.e., `var a = await t;`. This mean that wait until the Task is completed then return the result. Note that the method inside Task need to be async. 

     //TODO: Async modifier is used to specify that the method is asynchrnous (they run not in the order that we're expected)
     //TODO: Await operator suspends evaluation of the async method until the asynchronous operation completes
     //TODO: If you `await` for a method, that method must be async. The method that contains `await` must return a Task or Task<t>. Task<T> is used when we want to return a result of the method called. i.e., when reading a file, we want to return the file conent of the file, then we use Task<string>.
     //TODO: The method that is marked await must be async meaning that they have to return a Task or Task<T> (cannot be void, or any normal return type.)
     //TODO: A method returns Task does not require await to run. DO NOT think of the Task method like void method because there is no way to know when the void method will finish. However, with Task, it will return a Task to mark it as completed. 
     //TODO: Not every async method requires to return a Task. Async usually go with void but not with another normal types like string or double or int. 
     //TODO: Async void are usually used for event handler (where we don't need to be awaited by the caller).


     public bool IsConnected => _client != null && _client.IsConnected;



     public NetworkController()
     {
          _client = new NetworkConnection();
     }

     public void Connect(string serverAddress, int port, string name, World world)
     {
          if (string.IsNullOrWhiteSpace(name))
          {
               //TODO: Handling name
               return;
          }
          
          //Create a new connection
          _client = new NetworkConnection();

          _client.Connect(serverAddress, port);

          _client.Send(name);
          
          Debug.WriteLine("Connected");

          ProcessInput(world);
     }



     private void ProcessInput(World world)
     {
          
          HandleFirstLineInput(world);
          
          while (IsConnected) // Reading the input from the client
          {
               HandleInput(world);
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
                    Console.WriteLine("Check Player id");
                    Console.WriteLine(id);
                    world.playerID = id;
                    //world.SetPlayerId(id);
               }
               else
               {
                    Console.WriteLine("Cannot parse ID");
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
                    Console.WriteLine("Cannot parse size");
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

          Console.WriteLine("Current data: " + line);

          try
          {
               if (line.Contains("wall") && line.Contains("p1") && line.Contains("p2"))
               {
                    Wall? wall = JsonSerializer.Deserialize<Wall>(line);
                    if (wall != null)
                    {
                         world.AddWalls(wall);
                         
                    }

                    
               }else if (line.Contains("power") && line.Contains("loc"))
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
                         world.AddPlayer(player);
                    }

               }
          }
          catch(Exception e)
          {
               HandleError(e);
          }
     }




     private void HandleError(Exception e)
     {
          //TODO: Another way to handle error
          Console.WriteLine(e.Message);

          _client?.Disconnect();
          
          // client = null;
          
     }
     

     public void Disconnect()
     {
          _client?.Disconnect();
     }

     
     public void HandleMovement(string jsonstring)
     {
          
     }
}
using GUI.Components.Models;
using Microsoft.VisualBasic.CompilerServices;
using System.Text.Json;

namespace GUI.Components.Controllers;

public class NetworkController
{
     private NetworkConnection client;
     
     public World world;
     
     
     /// <summary>
     ///Store the current Task that working on. 
     /// </summary>
     private Task? _task;
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
     
     
     private bool IsConnected => client != null && client.IsConnected;
     
     public NetworkController()
     {
          world = new World();
          client =  new NetworkConnection();
     }

     public  Task ConnectAsync(string serverAddress, int port, string name)
     {
          world = new World();
          
          //Create a new connection
          client = new NetworkConnection();
          client.Connect(serverAddress, port);
          client.Send(name);

          _task = Task.Run(HandleInput);

          return _task;
     }



     private void HandleInput()
     {
          
          HandleFirstLineInput();
          
          // Handling walls, reading once
          HandleWallInput();


          while (IsConnected) // Reading the input from the client
          {
               HandleGameObject();
          }

          
     }

     private void HandleFirstLineInput()
     {
          string input;
          try
          {
               
               //Read the ID
               input = client.ReadLine();
               if (Int32.TryParse(input, out int id))
               {
                    world.SetPlayerID(id);
               }
               else
               {
                    Console.WriteLine("Cannot parse ID"); 
                    client.Disconnect(); 
               }
               
               //Read the size of the world
               
               input = client.ReadLine();

               if (Int32.TryParse(input, out int size))
               {
                    world.SetSize(size,size); // World are assumed to be square
               }
               else
               {
                    Console.WriteLine("Cannot parse size");
                    client.Disconnect();
               }
               
          }
          catch (Exception e) // Disconnect when there is an error occurred when reading from server
          {
               HandleError(e);
          }
     }


     private void HandleWallInput()
     {
          try
          {
               while (true)
               {
                    string line = client.ReadLine(); 
                    if (line.Contains("wall"))
                    {
                         Wall? wall = JsonSerializer.Deserialize<Wall>(line);
                         if (wall != null)
                         {
                              world.AddWalls(wall);
                         }
                    }
                    else
                    {
                         break;
                    }    
               }

               

          }
          catch (Exception e) // Can't read the walls anymore
          {
               HandleError(e);
          }
     }


     private void HandleError(Exception e)
     {
          //TODO: Another way to handle error
          Console.WriteLine(e.Message);

          client?.Disconnect();
          
          // client = null;

          _task = null;
     }

     private void HandleGameObject()
     {
          try
          {
               string line = client.ReadLine();
               if (line.Contains("snake")) // Handling player
               {
                    Player? player = JsonSerializer.Deserialize<Player>(line);
                    if (player != null)
                    {
                         world.AddPlayer(player);
                    }
               }
               else // Handling powerup
               {
                    Powerup? powerup = JsonSerializer.Deserialize<Powerup>(line);
                    if (powerup != null)
                    {
                         world.AddPowerup(powerup);
                    }
               }
          }
          catch (Exception e)
          {
               HandleError(e);
          }

     }

     // public void ConnectAndSendName(string name)
     // {
     //      if (name.Length > 16)
     //      {
     //           name = name.Substring(0, 16);
     //      }
     //
     //      if (name.Length == 0)
     //      {
     //           throw new Exception("Name cannot be empty");
     //      }
     //
     //      client.Send(name);
     // }

     public void HandleMovement(string jsonstring)
     {
          
     }
}
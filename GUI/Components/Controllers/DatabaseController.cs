using GUI.Components.Models;
using MySql.Data.MySqlClient;

namespace GUI.Components.Controllers;

public class DatabaseController
{
    private readonly string connectionString;

    public DatabaseController(string connectionString)
    {
       this.connectionString = connectionString;
    }
    
     public int UpdateStartTime(DateTime startTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               
               conn.Open();

               // Console.WriteLine("Database connection successful!");
               
               
               MySqlCommand command = conn.CreateCommand();
               
               command.CommandText =
                    @"INSERT INTO Game (StartTime)
                      VALUES (@startTime)";
               
               command.Parameters.AddWithValue("@startTime", startTime);
               
               command.ExecuteNonQuery();
               
               //Store the ID
               command.CommandText = "SELECT LAST_INSERT_ID();";
               return Convert.ToInt32(command.ExecuteScalar());
               
          }
     }

     public void UpdateEndTime(int gameID,  DateTime endTime)
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

     public void PrintGame()
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


     public void PrintPlayer()
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               try
               {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT ID, Name, StartTime, GameID, EndTime FROM Player";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                         while (reader.Read())
                         {
                              Console.WriteLine(
                                   reader["ID"] + " " +
                                   reader["Name"] + " " +
                                   reader["StartTime"] + " " +
                                   reader["GameID"] + " " +
                                   reader["EndTime"]
                              );
                         }
                    }
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }
          }
     }

     public void UpdatePlayerStartTime(World world, string playerName, int gameId,  DateTime startTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               
               conn.Open();
               
               MySqlCommand command = conn.CreateCommand();
               
               //
               Console.WriteLine($"World player id: {world.playerID}");
               
               if (!world.Players.ContainsKey(world.playerID)) // Not See the snake before, insert it
               {
                    command.CommandText = @"INSERT INTO Player (ID, Name, GameID, StartTime) VALUES (@id, @name, @gameId, @startTime)";

                    command.Parameters.AddWithValue("@id", world.playerID);
                    command.Parameters.AddWithValue("@name", playerName);
                    command.Parameters.AddWithValue("@gameId", gameId);
                    command.Parameters.AddWithValue("@startTime", startTime);
                    
                    command.ExecuteNonQuery();
               }
               else // See it before, update
               {
                    
               }
          }


     }


     public void UpdatePlayerEndTime(int gameId, DateTime endTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               
               conn.Open();
               
               MySqlCommand command = conn.CreateCommand();
               
               
               command.CommandText = @"UPDATE Player SET EndTime = @endTime WHERE GameID = @gameID";
               
               command.Parameters.AddWithValue("@endTime", endTime);
               
               command.Parameters.AddWithValue("@gameID", gameId);
               
               command.ExecuteNonQuery();
               
          } 
     }
}
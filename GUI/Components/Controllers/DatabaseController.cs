using MySql.Data.MySqlClient;

namespace GUI.Components.Controllers;

public class DatabaseController
{
    private readonly string connectionString;

    public DatabaseController(string connectionString)
    {
       this.connectionString = connectionString;
    }


    private int gameID;
    
     public void UpdateStartTime(DateTime startTime)
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
               gameID = Convert.ToInt32(command.ExecuteScalar());
               
          }
     }

     public void UpdateEndTime(int gameId,  DateTime endTime)
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
}
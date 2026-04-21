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

     public int UpdateGameStartTimeClient(DateTime startTime)
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

     public void UpdateGameEndTimeClient(int gameID, DateTime endTime)
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
     
    

     public void UpdateClientStartTime(World world, string playerName, int gameId, DateTime startTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               conn.Open();

               MySqlCommand command = conn.CreateCommand();

               //
               // Console.WriteLine($"World player id: {world.playerID}");


               if (!world.Players.ContainsKey(world.playerID)) // Not See the snake before, insert it
               {
                    command.CommandText =
                         @"INSERT INTO Player (ID, Name, GameID, StartTime) VALUES (@id, @name, @gameId, @startTime)";

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


     public void UpdateClientEndTime(int gameId, DateTime endTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();

               MySqlCommand command = conn.CreateCommand();


               command.CommandText = @"UPDATE Player SET EndTime = @endTime  WHERE GameID = @gameId";

               command.Parameters.AddWithValue("@endTime", endTime);

               command.Parameters.AddWithValue("@gameId", gameId);

               command.ExecuteNonQuery();

          }
     }

     public void UpdatePlayerMaxScore(int gameID, Player player)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               conn.Open();
               MySqlCommand command = conn.CreateCommand();

               command.CommandText = @"UPDATE Player SET MaxScore = @maxScore WHERE ID = @playerID AND @gameId = gameId";

               
               command.Parameters.AddWithValue("@gameId", gameID);
               command.Parameters.AddWithValue("@maxScore", player.MaxScore);
               command.Parameters.AddWithValue("@playerID", player.ID);

               command.ExecuteNonQuery();
          }
     }


     public void InsertPlayer(int gameId, Player player)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               conn.Open();
               MySqlCommand command = conn.CreateCommand();


               command.CommandText =
                    @"INSERT INTO Player (ID, Name, GameID, StartTime, MaxScore) VALUES (@id, @name, @gameId, @startTime, @maxScore)";

               command.Parameters.AddWithValue("@id", player.ID);
               command.Parameters.AddWithValue("@name", player.Name);
               command.Parameters.AddWithValue("@gameId", gameId);
               command.Parameters.AddWithValue("@startTime", player.EnterTime);
               command.Parameters.AddWithValue("@maxScore", player.MaxScore);
               command.ExecuteNonQuery();

               Console.WriteLine($"Updated Player  {player.Name} with ID {player.ID} started at {player.EnterTime} max score {player.MaxScore}");
          }
     }

     public void UpdatePlayerLeaveTime(int gameId, Player player)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               conn.Open();
               MySqlCommand command = conn.CreateCommand();
                    
               
               Console.WriteLine($"Before updating: GameID: {gameId}, playerID: {player.ID} LeaveTime: {player.LeaveTime}");
               command.CommandText = @"UPDATE Player SET EndTime = @endTime WHERE GameID = @gameID AND ID = @playerID";
               
               command.Parameters.AddWithValue("@gameID", gameId);
               command.Parameters.AddWithValue("@endTime", player.LeaveTime);
               command.Parameters.AddWithValue("@playerID", player.ID);

               command.ExecuteNonQuery();
               
               // Console.WriteLine("Updated Player " + player.ID + " LeaveTime with" + player.LeaveTime);
          }
     }


     public int UpdateGameStartTime(Player player)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();



               MySqlCommand command = conn.CreateCommand();

               command.CommandText =
                    @"INSERT INTO Game (StartTime)
                      VALUES (@startTime)";

               command.Parameters.AddWithValue("@startTime", player.EnterTime);

               command.ExecuteNonQuery();

               //Store the ID
               command.CommandText = "SELECT LAST_INSERT_ID();";

               Console.WriteLine("Updated Game Start Time");
               return Convert.ToInt32(command.ExecuteScalar());

          }
     }


     public void UpdateGameEndTime(int gameId, Player player)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();

               MySqlCommand command = conn.CreateCommand();

               //Update the existing row
               command.CommandText =
                    @"Update Game set EndTime = @endTime WHERE ID = @gameID";

               command.Parameters.AddWithValue("@endTime", player.LeaveTime);
               command.Parameters.AddWithValue("@gameID", gameId);


               command.ExecuteNonQuery();


               Console.WriteLine("Updated " + player.Name + " EndTime with" + player.LeaveTime);

          }
     }


     public List<GameRecord> GetGameRecords()
     {
          List<GameRecord> games = new List<GameRecord>();


          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();

               MySqlCommand command = conn.CreateCommand();
               
               command.CommandText = @"SELECT ID, StartTime, EndTime FROM Game";
               
               
               using ( MySqlDataReader reader = command.ExecuteReader() )
               {
                    while ( reader.Read() )
                    {
                        GameRecord game = new GameRecord();
                        game.GameId = reader.GetInt32( 0 );
                        game.StartTime = reader.GetDateTime( 1 );

                        if (!reader.IsDBNull(2)) // Add when the endtime is not null
                        {
                             game.EndTime = reader.GetDateTime( 2 ) ;
                             
                        }

                        games.Add( game );
                    }
               }
               
               
          }

          return games;
     }



}
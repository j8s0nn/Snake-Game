// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/21/2026 </date>
//        Represents the database controller responsible for managing game and player data,
//        including inserting, updating, and retrieving records from the database.
//   </para>
// </summary>


using GUI.Components.Models;
using MySql.Data.MySqlClient;

namespace GUI.Components.Controllers;



/// <summary>
/// Provides database operations for storing and retrieving game and player data
/// for the Snake game application.
/// </summary>
/// <remarks>
/// This controller is responsible for:
/// <list type="bullet">
/// <item>Creating and updating game records (start and end times)</item>
/// <item>Inserting and updating player records (scores, join/leave times)</item>
/// <item>Retrieving stored game and player statistics</item>
/// </list>
/// </remarks>
public class DatabaseController
{
     
     /// <summary>
     /// The connection string used to connect to the database.
     /// </summary>
     private readonly string connectionString;

     
     /// <summary>
     /// Initializes a new instance of the <see cref="DatabaseController"/> class.
     /// </summary>
     /// <param name="connectionString">
     /// The connection string used to connect to the MySQL database.
     /// </param>
     public DatabaseController(string connectionString)
     {
          this.connectionString = connectionString;
     }

     
     /// <summary>
     /// Inserts a new game record with the specified start time when the client connect to the game.
     /// </summary>
     /// <param name="startTime">The time the game started.</param>
     /// <returns>The ID of the newly created game.</returns>
     public int UpdateGameStartTimeClient(DateTime startTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();

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

     
     /// <summary>
     /// Updates the end time of a game when the client disconnect.
     /// </summary>
     /// <param name="gameID">The ID of the game to update.</param>
     /// <param name="endTime">The time the game ended.</param>
     public void UpdateGameEndTimeClient(int gameID, DateTime endTime)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();
               
               MySqlCommand command = conn.CreateCommand();

               //Update the existing row
               command.CommandText =
                    @"Update Game set EndTime = @endTime WHERE ID = @gameID";

               command.Parameters.AddWithValue("@endTime", endTime);
               command.Parameters.AddWithValue("@gameID", gameID);

               command.ExecuteNonQuery();

          }
     }
     
     
     /// <summary>
     /// Updates the maximum score of a player within a specific game.
     /// </summary>
     /// <param name="gameID">The ID of the game.</param>
     /// <param name="player">The player whose score is updated.</param>
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


     /// <summary>
     /// Inserts a new player record when a player is first seen in a game.
     /// </summary>
     /// <param name="gameId">The ID of the game.</param>
     /// <param name="player">The player to insert.</param>
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
          }
     }

     
     /// <summary>
     /// Updates the leave time (EndTime) for a player in a specific game. 
     /// </summary>
     /// <param name="gameId">The ID of the game.</param>
     /// <param name="player">The player who disconnected.</param>
     /// <remarks>
     /// This method is used when the client receives a player update from the server where the "dc" property of a a player is set to true.
     /// </remarks>
     public void UpdatePlayerLeaveTime(int gameId, Player player)
     {
          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
               conn.Open();
               MySqlCommand command = conn.CreateCommand();
               
               command.CommandText = @"UPDATE Player SET EndTime = @endTime WHERE GameID = @gameID AND ID = @playerID";
               
               command.Parameters.AddWithValue("@gameID", gameId);
               command.Parameters.AddWithValue("@endTime", player.LeaveTime);
               command.Parameters.AddWithValue("@playerID", player.ID);

               command.ExecuteNonQuery();
               
          }
     }


     /// <summary>
     /// Retrieves all game records from the database.
     /// </summary>
     /// <returns>A list of <see cref="GameRecord"/> objects.</returns>
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

     
     /// <summary>
     /// Retrieves all player records for a specific game.
     /// </summary>
     /// <param name="gameId">The ID of the game.</param>
     /// <returns>A list of <see cref="PlayerRecord"/> objects.</returns>
     public List<PlayerRecord> GetPlayerRecords(int gameId)
     {
          List<PlayerRecord> players = new List<PlayerRecord>();


          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {

               conn.Open();

               MySqlCommand command = conn.CreateCommand();
               
               //Select from a specific game
               command.CommandText = @"SELECT ID, Name, MaxScore, StartTime, EndTime FROM Player WHERE GameId =@gid";
               
               command.Parameters.AddWithValue("@gid", gameId);
               
               using ( MySqlDataReader reader = command.ExecuteReader() )
               {
                    while ( reader.Read() )
                    {
                         PlayerRecord player = new PlayerRecord();
                         player.ID = reader.GetInt32( "ID" );
                         player.Name = reader.GetString( "Name" );
                         player.MaxScore = reader.GetInt32( "MaxScore" );
                         player.StartTime = reader.GetDateTime( "StartTime" );

                         int endTimeIndex = reader.GetOrdinal("EndTime");
                         if (!reader.IsDBNull(endTimeIndex)) // Add when the endtime is not null
                         {
                              player.EndTime = reader.GetDateTime( "EndTime") ;
                             
                         }

                         players.Add( player );
                    }
               }
          }

          return players;
     }
     

}
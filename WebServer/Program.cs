// <summary>
//   <para>
//     <authors> Quoc Thinh Le </authors>
//     <date> 4/21/2026 </date>
//     Provides a simple HTTP server that displays game and player statistics
//     retrieved from the Snake game database.
//   </para>
// </summary>


using System.Text;
using CS3500.Networking;
using GUI.Components.Controllers;
using GUI.Components.Models;

class Program 
{
    /// <summary>
    /// Represents the HTTP response header sent when a request is successfully processed (HTTP 200 OK).
    /// </summary>
    private static string httpOkHeader =
        "HTTP/1.1 200 OK\r\n" +
        "Connection: close\r\n" +
        "Content-Type: text/html; charset=UTF-8\r\n" +
        "\r\n";

    /// <summary>
    /// Represents the HTTP response header sent when a requested resource is not found (HTTP 404).
    /// </summary>
    private static string httpErrorHeader =
        "HTTP/1.1 404 Not Found\r\n" +
        "Connection: close\r\n" +
        "Content-Type: text/html; charset=UTF-8\r\n" +
        "\r\n";
    
    /// <summary>
    /// Entry point of the application. Starts the HTTP server and listens for incoming client requests.
    /// </summary>
    /// <param name="args">Not used in this case</param>
    static void Main(string[] args)
    {
        //Server is on http://localhost:8080/
        Server.StartServer(HandleConnection, 8080);
    }

    
    /// <summary>
    /// Handles an incoming HTTP connection and serves game statistics from the database.
    /// </summary>
    /// <param name="handleConnection">
    /// The network connection representing the incoming client HTTP request.
    /// </param>
    static void HandleConnection(NetworkConnection handleConnection)
    {
        
        string connectionString =
            "server=atr.eng.utah.edu;" +
            "database=u1579771;" +
            "uid=u1579771;" +
            "password=0974107730";

        try
        {
            string httpLine = handleConnection.ReadLine();


            DatabaseController databaseController = new DatabaseController(connectionString);


            if (!httpLine.Contains("GET"))
            {
                //Error happens
                handleConnection.Send(httpErrorHeader);
                handleConnection.Disconnect();
            }


            string[] splittedHttpRequest = httpLine.Split(' ');

            //GET / HTTP/1.1 

            //GET /Players HTTP/1.1 

            if (splittedHttpRequest[1].Trim() == "/") // Home page
            {
                handleConnection.Send(httpOkHeader +
                                      "<html>\n  <h3>Welcome to the Snake Games Database!</h3>\n  <a href=\"/games\">View Games</a>\n</html>");
                handleConnection.Disconnect();
            }
            else if (splittedHttpRequest[1].Trim().StartsWith("/games?gid=")) 
            {
                
                //Extract the gameId
                string[] parts = splittedHttpRequest[1].Split('=');
                if (parts.Length < 2 || !int.TryParse(parts[1], out int gid))
                {
                    handleConnection.Send(httpErrorHeader + "<html>Invalid Game ID</html>");
                    handleConnection.Disconnect();
                    return;
                }
                
                //Get the player records
                List<PlayerRecord> playerRecords = databaseController.GetPlayerRecords(gid);
                
                StringBuilder content = new StringBuilder();
                
                content.AppendLine("<html>");
                content.AppendLine($"<h3>Stats for Game {gid}</h3>");
                content.AppendLine("<table border=\"1\">");
                content.AppendLine("<thead>");
                content.AppendLine("<tr>");
                content.AppendLine("<td>Player ID</td><td>Player Name</td><td>Max Score</td><td>Enter Time</td><td>Leave Time</td>");
                content.AppendLine("</tr>");
                content.AppendLine("</thead>");
                content.AppendLine("<tbody>");
                
                
                foreach (PlayerRecord playerRecord in playerRecords)
                {
                    content.AppendLine("<tr>");

                    //Handling nullable endtime
                    string leaveTime = playerRecord.EndTime?.ToString() ?? string.Empty;

                    content.Append($"<td>{playerRecord.ID}</td><td>{playerRecord.Name}</td><td>{playerRecord.MaxScore}</td><td>{playerRecord.StartTime}</td><td>{leaveTime}</td>");
                    content.AppendLine("</tr>");
                }
                
                content.Append("</tbody>");
                content.AppendLine("</table>");
                content.AppendLine("<br/><a href='/games'>Back to Games List</a>");
                content.AppendLine("</html>");
                
                handleConnection.Send(httpOkHeader + content.ToString());
                handleConnection.Disconnect();

            }
            else // Stats for specific game
            {
                List<GameRecord> games = databaseController.GetGameRecords();
                
                
                //Create a string for rendering
                StringBuilder content = new StringBuilder();
                content.AppendLine("<html>");
                content.AppendLine("<table border=\"1\">");
                content.AppendLine("<thead>");
                content.AppendLine("<tr>");
                content.AppendLine("<td>ID</td><td>Start</td><td>End</td>");
                content.AppendLine("</tr>");
                content.AppendLine("</thead>");

                //Create each row for each game
                foreach (GameRecord game in games)
                {   
                    int gameId = game.GameId;
                    DateTime startTime = game.StartTime;
                    DateTime? endTime = game.EndTime;
                    
                    content.AppendLine("<tr>");
                    content.AppendLine($"<td><a href='/games?gid={gameId}'>{gameId}</a></td>");
                    content.AppendLine($"<td>{startTime}</td>");
                    content.AppendLine($"<td>{endTime}</td>");
                    content.AppendLine("</tr>");
                }
                
                content.AppendLine("</tbody>");
                content.AppendLine("</table>");
                content.AppendLine("</html>");
                
                handleConnection.Send(httpOkHeader+ content.ToString());

                handleConnection.Disconnect();
                
                
            }


        }
        catch (Exception e)
        {
            handleConnection.Send(httpErrorHeader);
            handleConnection.Disconnect();
        }


    }


}
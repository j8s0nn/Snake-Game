// See https://aka.ms/new-console-template for more information

using System.Text;
using CS3500.Networking;
using GUI.Components.Controllers;
using MySql.Data.MySqlClient;
using GUI.Components.Controllers;
using GUI.Components.Models;

class Program //TODO: http://localhost:8080/
{
    private static string httpOkHeader =
        "HTTP/1.1 200 OK\r\n" +
        "Connection: close\r\n" +
        "Content-Type: text/html; charset=UTF-8\r\n" +
        "\r\n";

    private static string httpErrorHeader =
        "HTTP/1.1 404 Not Found\r\n" +
        "Connection: close\r\n" +
        "Content-Type: text/html; charset=UTF-8\r\n" +
        "\r\n";

    static void Main(string[] args)
    {
        Server.StartServer(HandleConnection, 8080);
    }

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
            else if (splittedHttpRequest[1].Trim() == "/games")
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
            else // Stats for specific game
            {

            }


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }


    }


}
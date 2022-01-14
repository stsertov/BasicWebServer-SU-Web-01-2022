namespace BasicWebServer.Demo
{

    using BasicWebServer.Server;
    using BasicWebServer.Server.HTTP;
    using BasicWebServer.Server.Responses;
    using System.Text;

    public class StartUp
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";
        public static void Main()
        {
            Console.WriteLine(Ragnarok());

            HttpServer server = new HttpServer(routes => routes
                            .MapGet("/", new TextResponse($"{Ragnarok()}"))
                            .MapGet("/HTML", new HtmlResponse(HtmlForm))
                            .MapGet("/Redirect", new RedirectResponse("https://jf-alcobertas.pt/img/entertainment/14/ragnarok-season-2-what-are-fans-expecting.jpeg"))
                            .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction)));
            server.Start();
        }
        public static string Ragnarok()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
            sb.AppendLine(@"\\        \\\\\\\ \\\\\\\\     \\\\  \\\\\  \\\\\\ \\\\\\       \\\\\\     \\\\  \\\\\  \\");
            sb.AppendLine(@"\\  \\\\  \\\\\\   \\\\\   \\\\  \\   \\\\  \\\\\   \\\\\  \\\\  \\\   \\\  \\\  \\\  \\\\");
            sb.AppendLine(@"\\  \\\\  \\\\\  \  \\\\  \\\\\\\\\    \\\  \\\\  \  \\\\  \\\\  \\\  \\\\\  \\  \\  \\\\\");
            sb.AppendLine(@"\\       \\\\\  \\\  \\\  \\\    \\  \  \\  \\\  \\\  \\\       \\\\  \\\\\  \\  \   \\\\\");
            sb.AppendLine(@"\\  \\\\  \\\         \\   \\\\  \\  \\  \  \\         \\  \\\\  \\\   \\\  \\\  \\\   \\\");
            sb.AppendLine(@"\\  \\\\\  \\  \\\\\  \\\\      \\\  \\\    \\  \\\\\  \\  \\\\\  \\\\     \\\\  \\\\\  \\");
            sb.AppendLine(@"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");

            return sb.ToString();
        }

        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = string.Empty;

            foreach (var (name, value) in request.Form)
            {
                response.Body += $"{name} - {value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}
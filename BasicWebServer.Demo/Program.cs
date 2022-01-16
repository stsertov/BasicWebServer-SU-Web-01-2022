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

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private const string FileName = "content.txt";
        public static async Task Main()
        {
            Console.WriteLine(Ragnarok());

            await DownloadSitesAsTextFile(StartUp.FileName, new string[] { "https://judge.softuni.bg", "https://softuni.org" });

            var server = new HttpServer(routes => routes
                            .MapGet("/", new TextResponse($"{Ragnarok()}"))
                            .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
                            .MapGet("/Redirect", new RedirectResponse("https://bit.ly/3rr1qWS"))
                            .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction))
                            .MapGet("/Content", new HtmlResponse(StartUp.DownloadForm))
                            .MapPost("/Content", new TextFileResponse(StartUp.FileName)));

            await server.Start();
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

        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();
            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var responses = await Task.WhenAll(downloads);

            var responseString = string.Join(Environment.NewLine + new string('-', 100), responses);

            await File.WriteAllTextAsync(fileName, responseString);
        }
    }
}
namespace BasicWebServer.Demo
{

    using BasicWebServer.Server;
    using BasicWebServer.Server.HTTP;
    using BasicWebServer.Server.Responses;
    using System.Text;
    using System.Web;

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

        private const string LoginForm = @"<form action='/Login' method='POST'>
   Username: <input type='text' name='Username'/>
   Password: <input type='text' name='Password'/>
   <input type='submit' value ='Log In' /> 
</form>";

        private const string Username = "user";

        private const string Password = "user123";

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
                            .MapPost("/Content", new TextFileResponse(StartUp.FileName))
                            .MapGet("/Cookies", new HtmlResponse("", StartUp.AddCookiesAction))
                            .MapGet("/Session", new TextResponse("", StartUp.DisplaySessionInfoAction))
                            .MapGet("/Login", new HtmlResponse(StartUp.LoginForm))
                            .MapPost("/Login", new HtmlResponse("", StartUp.LoginAction))
                            .MapGet("/Logout", new HtmlResponse("", StartUp.LogoutAction))
                            .MapGet("/UserProfile", new HtmlResponse("", StartUp.GetUserData)));

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

        private static void AddCookiesAction(Request request, Response response)
        {
            var requestHasCookies = request.Cookies
                .Any(c => c.Name != Session.SessionCookieName);
            var bodyText = string.Empty;

            if (requestHasCookies)
            {
                var cookieText = new StringBuilder();
                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.Append("<table border ='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }
                cookieText.Append("</table>");

                bodyText = cookieText.ToString();
            }
            else
            {
                bodyText = "<h1>Cookies set!</h1>";

                response.Cookies.Add("My-Cookie", "My-Value");
                response.Cookies.Add("Ragnarok-Cookie", "Ragnarok-Value");
            }

            response.Body = bodyText;
        }

        private static void DisplaySessionInfoAction(Request request, Response response)
        {
            var sessionExists = request.Session
                .ContainsKey(Session.SessionCurrentDateKey);

            var bodyText = string.Empty;

            if (sessionExists)
            {
                var currentDate = request.Session[Session.SessionCurrentDateKey];

                bodyText = $"Date stored: {currentDate}";
            }
            else
            {
                bodyText = "Current date stored!";
            }

            response.Body = string.Empty;
            response.Body = bodyText;
        }

        private static void LoginAction(Request request, Response response)
        {
            request.Session.Clear();

            var bodyText = string.Empty;

            var usernameMatches = request.Form["Username"] == StartUp.Username;
            var passwordMatches = request.Form["Password"] == StartUp.Password;

            if (usernameMatches && passwordMatches)
            {
                request.Session[Session.SessionUserKey] = "MyUserIDFromDB";
                response.Cookies
                    .Add(Session.SessionCookieName, request.Session.Id);

                bodyText = "<h3>Logged successfully!</h3>";
            }
            else
            {
                bodyText = StartUp.LoginForm;
            }

            response.Body = string.Empty;
            response.Body = bodyText;
        }

        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();

            response.Body = string.Empty;
            response.Body = "<h3>Logged out successfully!</h3>";
        }

        private static void GetUserData(Request request, Response response)
        {
            var sessionExists = request.Session
                .ContainsKey(Session.SessionUserKey);
            response.Body = string.Empty;

            if (sessionExists)
            {
                response.Body = $"<h3>Currently logged-in user is with username '{StartUp.Username}'</h3>";
            }
            else
            {
                response.Body = $"<h3>You should first log in - <a href='/Login'>Login</a></h3>";
            }
        }
    }
}
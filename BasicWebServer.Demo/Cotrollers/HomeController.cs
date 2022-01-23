﻿namespace BasicWebServer.Demo.Cotrollers
{
    using BasicWebServer.Demo.Models;
    using BasicWebServer.Server.Controllers;
    using BasicWebServer.Server.HTTP;
    using System.Text;
    using System.Web;

    public class HomeController : Controller
    {
        private const string FileName = "content.txt";

        public HomeController(Request _request)
            : base(_request)
        {
        }

        public Response Index(string text) => Text(text);

        public Response Redirect() => Redirect(@"https://softuni.org/");

        public Response Html() => View();

        public Response HtmlFormPost()
        {
            var name = Request.Form["Name"];
            var age = Request.Form["Age"];

            var model = new FormViewModel()
            {
                Name = name,
                Age = int.Parse(age)
            };

            return View(model);
        }

        public Response Content() => View();

       // public Response Content() => Html(HomeController.DownloadForm);

        public Response DownloadContent()
        {
            DownloadSitesAsTextFile(HomeController.FileName,
                new[] { "https://judge.softuni.org/", "https://softuni.org/" })
                .Wait();

            return File(HomeController.FileName);
        }

        public Response Cookies()
        {
            if (Request.Cookies
                .Any(c => c.Name != Session.SessionCookieName))
            {
                var cookieText = new StringBuilder();
                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.Append("<table border ='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in Request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }
                cookieText.Append("</table>");

                return Html(cookieText.ToString());
            }

            var cookies = new CookieCollection();

            cookies.Add("My-Cookie", "My-Value");
            cookies.Add("Ragnarok-Cookie", "Ragnarok-Value");

            return Html("<h1>Cookies set!</h1>", cookies);
        }

        public Response SessionHome()
        {
            string currentDateKey = "CurrentDate";
            var sessionExists = Request.Session
                .ContainsKey(currentDateKey);

            if(sessionExists)
            {
                var currentDate = Request.Session[currentDateKey];
                return Text($"Stored date: {currentDate}!");
            }

            return Text("Current date stored!");
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

            await System.IO.File.WriteAllTextAsync(fileName, responseString);
        }
    }
}

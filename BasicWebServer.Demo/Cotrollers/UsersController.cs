namespace BasicWebServer.Demo.Cotrollers
{
    using BasicWebServer.Server.Controllers;
    using BasicWebServer.Server.HTTP;

    public class UsersController : Controller
    {
        private const string Username = "user";

        private const string Password = "user123";

        public UsersController(Request _request)
            : base(_request)
        {
        }

        public Response Login() => View();

        public Response LogInUser()
        {
            Request.Session.Clear();

            var usernameMatches = Request.Form["Username"] == UsersController.Username;
            var passwordMatches = Request.Form["Password"] == UsersController.Password;

            if (usernameMatches && passwordMatches)
            {

                if (!Request.Session.ContainsKey(Session.SessionUserKey))
                {
                    Request.Session[Session.SessionUserKey] = "MyUserIDFromDB";
                    var cookies = new CookieCollection();

                    cookies.Add(Session.SessionCookieName, Request.Session.Id);
                    return Html("<h3>Logged successfully!</h3>", cookies);
                }

                return Html("<h3>Logged successfully!</h3>");
            }
            return Redirect("/Login");
        }

        public Response Logout()
        {
            Request.Session.Clear();

            return Html("<h3>Logged out successfully!</h3>");
        }

        public Response GetUserData()
        {
            if (Request.Session
                .ContainsKey(Session.SessionUserKey))
            {
                return Html($"<h3>Currently logged-in user is with username '{UsersController.Username}'</h3>");
            }

            return Redirect("/Login");
        }
    }
}

namespace BasicWebServer.Server.HTTP
{
    using System.Web;
    public class Request
    {
        private const string separator = "\r\n";
        public Method Method { get; private set; }

        public string Url { get; private set; }

        public HeaderCollection Headers { get; private set; }

        public CookieCollection Cookies { get; private set; }

        public IReadOnlyDictionary<string, string> Form { get; private set; }
        public string Body { get; private set; }

        public static Request Parse(string request)
        {
            var lines = request.Split(separator);

            var firstLine = lines
                .First()
                .Split(" ");

            var method = ParseMethod(firstLine[0]);
            var url = firstLine[1];

            var headers = ParseHeaders(lines.Skip(1));

            var cookies = ParseCookies(headers);

            string body = string.Join(separator, lines.Skip(headers.Count + 2));

            var form = ParseForm(headers, body);

            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Cookies = cookies,
                Form = form,
                Body = body
            };
        }
        private static Method ParseMethod(string method)
        {
            try
            {
                return Enum.Parse<Method>(method);
            }
            catch (Exception)
            {

                throw new InvalidOperationException($"Method '{method}' is not supported!");
            };
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headers = new HeaderCollection();

            foreach (var line in headerLines)
            {
                if (line == string.Empty)
                {
                    break;
                }

                var headerParts = line.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid.");
                }

                headers.Add(headerParts[0], headerParts[1].Trim());
            }
            return headers;
        }

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookieCollection = new CookieCollection();

            if (headers.Contains(Header.Cookie))
            {
                var cookieHeader = headers[Header.Cookie].Value;
                var allCookies = cookieHeader.Split(';');

                foreach (var cookieText in allCookies)
                {
                    var cookieParts = cookieText.Split('=');

                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();

                    cookieCollection.Add(cookieName, cookieValue);
                }
            }
            return cookieCollection;
        }

        private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var form = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType].Value == ContentType.FormUrlEncoded)
            {
                var resultForm = ParseFormData(body);

                foreach (var (name, value) in resultForm)
                {
                    form.Add(name, value);
                }
            }
            return form;
        }

        private static Dictionary<string, string> ParseFormData(string body)
            => HttpUtility.UrlDecode(body)
            .Split('&')
            .Select(x => x.Split('='))
            .Where(x => x.Length == 2)
            .ToDictionary(
                key => key[0],
                value => value[1],
                StringComparer.InvariantCultureIgnoreCase);
    }
}

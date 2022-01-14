namespace BasicWebServer.Server.Routing
{
    using BasicWebServer.Server.Common;
    using BasicWebServer.Server.HTTP;
    using BasicWebServer.Server.Responses;

    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Response>> routes = new()
        {
            [Method.GET] = new(),
            [Method.POST] = new(),
            [Method.PUT] = new(),
            [Method.DELETE] = new()
        };

        public IRoutingTable Map(string url, Method method, Response response)
         => method switch
         {
             Method.GET => MapGet(url, response),
             Method.POST => MapPost(url, response),
             _ => throw new InvalidOperationException($"Method '{method}' is not supported.")
         };

        public IRoutingTable MapGet(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            routes[Method.GET][url] = response;

            return this;
        }

        public IRoutingTable MapPost(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            routes[Method.POST][url] = response;

            return this;
        }

        public Response MatchRequest(Request request)
        {
            var method = request.Method;
            var url = request.Url;

            if(!routes.ContainsKey(method) || !routes[method].ContainsKey(url))
            {
                return new NotFoundResponse();
            }

            return routes[method][url];
        }
    }
}

namespace BasicWebServer.Server
{
    using BasicWebServer.Server.HTTP;
    using BasicWebServer.Server.Routing;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;
        private readonly RoutingTable routingTable;

        public HttpServer(string _ipAddress,
            int _port,
            Action<IRoutingTable> _routingTableConfiguration)
        {
            ipAddress = IPAddress.Parse(_ipAddress);
            port = _port;
            serverListener = new TcpListener(ipAddress, port);

            _routingTableConfiguration(routingTable = new RoutingTable());
        }

        public HttpServer(int _port, Action<IRoutingTable> _routingTableConfiguration)
            : this("127.0.0.1", _port, _routingTableConfiguration)
        {
        }

        public HttpServer(Action<IRoutingTable> _routingTableConfiguration)
            : this(5000, _routingTableConfiguration)
        {
        }

        public async Task Start()
        {
            serverListener.Start();

            Console.WriteLine($"Server started on port {port}!");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = await serverListener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();

                    var requestText = await ReadRequest(networkStream);

                    Console.WriteLine(requestText);

                    var request = Request.Parse(requestText);

                    var response = routingTable.MatchRequest(request);

                    AddSession(request, response);

                    await WriteResponse(networkStream, response);

                    connection.Close();
                });
            }
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];
            var totalBytes = 0;

            var sb = new StringBuilder();

            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);
                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            while (networkStream.DataAvailable);

            return sb.ToString();
        }

        private static void AddSession(Request request, Response response)
        {
            var sessionExists = request.Session
                .ContainsKey(Session.SessionCurrentDateKey);

            if(!sessionExists)
            {
                request.Session[Session.SessionCurrentDateKey]
                    = DateTime.Now.ToString();

                response.Cookies
                    .Add(Session.SessionCookieName, request.Session.Id);
            }
        }

        private async Task WriteResponse(NetworkStream networkStream, Response response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            await networkStream.WriteAsync(responseBytes);
        }
    }
}

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
            Action<IRoutingTable> routingTableConfiguration)
        {
            ipAddress = IPAddress.Parse(_ipAddress);
            port = _port;
            serverListener = new TcpListener(ipAddress, port);

            routingTableConfiguration(routingTable = new RoutingTable());
        }

        public HttpServer(int _port, Action<IRoutingTable> routingTableConfiguration)
            : this("127.0.0.1", _port, routingTableConfiguration)
        { 
        }

        public HttpServer(Action<IRoutingTable> routingTableConfiguration)
            : this(5000, routingTableConfiguration)
        { 
        }

        public void Start()
        {
            serverListener.Start();

            Console.WriteLine($"Server started on port {port}!");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = serverListener.AcceptTcpClient();

                var networkStream = connection.GetStream();

                var requestText = ReadRequest(networkStream);

                Console.WriteLine(requestText);

                var request = Request.Parse(requestText);

                var response = routingTable.MatchRequest(request);

                if(response.PreRenderAction != null)
                {
                    response.PreRenderAction(request, response);
                }

              //  var content = "Hello from the Server!";

                WriteResponse(networkStream, response);

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, Response response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            networkStream.Write(responseBytes);
        }

        private string ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];
            var totalBytes = 0;

            var sb = new StringBuilder();

            do
            {
                var bytesRead = networkStream.Read(buffer, 0, bufferLength);
                totalBytes += bytesRead;

                if(totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } 
            while (networkStream.DataAvailable);

            return sb.ToString();
        }

    }
}

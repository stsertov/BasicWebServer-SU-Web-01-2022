namespace BasicWebServer.Server.HTTP
{
    using System.Text;
    public class Response
    {
        public Response(StatusCode _statusCode)
        {
            StatusCode = _statusCode;
            Headers.Add(Header.Server, "Ragnarok");
            Headers.Add(Header.Date, $"{DateTime.UtcNow:r}");
        }

        public StatusCode StatusCode { get; init; }

        public HeaderCollection Headers { get; } = new HeaderCollection();
        public string Body { get; set; }

        public Action<Request, Response> PreRenderAction { get; protected set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach (var header in this.Headers)
            {
                sb.AppendLine(header.ToString());
            }

            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(this.Body))
            {
                sb.AppendLine(this.Body);
            }

            return sb.ToString();
        }
    }
}

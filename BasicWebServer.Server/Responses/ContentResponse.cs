namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.Common;
    using BasicWebServer.Server.HTTP;
    using System.Text;

    public class ContentResponse : Response
    {
        public ContentResponse(string _content, string _contentType) 
            : base(StatusCode.OK)
        {
            Guard.AgainstNull(_content);
            Guard.AgainstNull(_contentType);

            Headers.Add(Header.ContentType, _contentType);

            Body = _content;
        }

        public override string ToString()
        {
            if(this.Body != null)
            {
                var contentLength = Encoding.UTF8.GetByteCount(Body).ToString();
                Headers.Add(Header.ContentLength, contentLength);
            }
            return base.ToString();
        }
    }
}

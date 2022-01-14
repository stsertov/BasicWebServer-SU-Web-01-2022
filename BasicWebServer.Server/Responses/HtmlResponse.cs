namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string _content) 
            : base(_content, ContentType.Html)
        {
        }
    }
}

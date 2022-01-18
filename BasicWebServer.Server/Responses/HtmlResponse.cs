namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string _content, Action<Request, Response> _preRenderAction = null) 
            : base(_content, ContentType.Html, _preRenderAction)
        {
            
        }
    }
}

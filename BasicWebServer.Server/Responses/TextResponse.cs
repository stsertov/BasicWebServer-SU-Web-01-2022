namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    public class TextResponse : ContentResponse
    {
        public TextResponse(string _content, Action<Request, Response> preRenderAction = null) 
            : base(_content, ContentType.PlainText, preRenderAction)
        {
        }
    }
}

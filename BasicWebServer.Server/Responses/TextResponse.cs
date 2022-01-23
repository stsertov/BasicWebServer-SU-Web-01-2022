namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    public class TextResponse : ContentResponse
    {
        public TextResponse(string _content) 
            : base(_content, ContentType.PlainText)
        {
        }
    }
}

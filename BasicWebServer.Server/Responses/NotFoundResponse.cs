namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    internal class NotFoundResponse : Response
    {
        public NotFoundResponse() 
            : base(StatusCode.NotFound)
        {
        }
    }
}

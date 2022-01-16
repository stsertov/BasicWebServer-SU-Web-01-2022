namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.Common;
    using BasicWebServer.Server.HTTP;

    public class TextFileResponse : Response
    {
        public TextFileResponse(string _fileName) 
            : base(StatusCode.OK)
        {
            Guard.AgainstNull(_fileName, nameof(_fileName));
            FileName= _fileName;

            this.Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public string FileName { get; init; }

        public override string ToString()
        {
            if(File.Exists(FileName))
            {
                this.Body = File.ReadAllTextAsync(FileName).Result;

                var bytesCount = (new FileInfo(FileName).Length).ToString();
                Headers.Add(Header.ContentLength, bytesCount);

                Headers.Add(Header.ContentDisposition, $"attachment; filename=\"{FileName}\"");
            }

            return base.ToString();
        }
    }
}

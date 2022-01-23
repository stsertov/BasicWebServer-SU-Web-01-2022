namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;
    public class ViewResponse : ContentResponse
    {
        private const char PathSeparator = '/';
        public ViewResponse(string _viewName,
            string _controllerName,
            object model = null)
            : base("", ContentType.Html)
        {
            if (!_viewName.Contains(PathSeparator))
            {
                _viewName = _controllerName + PathSeparator + _viewName;
            }

            var viewPath = Path.GetFullPath($"./Views/" + _viewName.TrimStart(PathSeparator) + ".cshtml");

            var viewContent = File.ReadAllText(viewPath);

            if (model != null)
            {
                viewContent = PopulateModel(viewContent, model);
            }

            Body = viewContent;
        }

        private string PopulateModel(string viewContent, object model)
        {
            var data = model
                .GetType()
                .GetProperties()
                .Select(pr => new
                {
                    pr.Name,
                    Value = pr.GetValue(model)
                });

            foreach(var info in data)
            {
                const string openingBrackets = "{{";
                const string closingBrackets = "}}";

                viewContent = viewContent.Replace($"{openingBrackets}{info.Name}{closingBrackets}",
                    info.Value.ToString());
            }

            return viewContent;
        }
    }
}

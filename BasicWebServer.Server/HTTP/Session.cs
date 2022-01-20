namespace BasicWebServer.Server.HTTP
{
    using BasicWebServer.Server.Common;
    using System.Collections.Generic;
    public class Session
    {
        public const string SessionCookieName = "RagnarokSID";

        public const string SessionCurrentDateKey = "CurrentDate";

        public const string SessionUserKey = "AuthenticatedUserId";

        private readonly Dictionary<string, string> data;

        public Session(string _id)
        {
            Guard.AgainstNull(_id, nameof(_id));

            Id = _id;

            data = new();
        }

        public string Id { get; set; }

        public string this[string key]
        {
            get => data[key];
            set => data[key] = value;
        }

        public bool ContainsKey(string key)
            => data.ContainsKey(key);

        public void Clear()
            => data.Clear();
    }
}

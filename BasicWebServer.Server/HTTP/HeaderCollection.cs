namespace BasicWebServer.Server.HTTP
{
    using System.Collections;
    using System.Collections.Generic;
    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> headers;

        public HeaderCollection()
            => this.headers = new Dictionary<string, Header>();
        public int Count 
            => this.headers.Count;

        public Header this[string key] => headers[key];

        public void Add(string name, string value)
            => this.headers[name] = new Header(name, value);

        public bool ContainsKey(string key)
            => this.headers.ContainsKey(key);
        public IEnumerator<Header> GetEnumerator()
            => this.headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}

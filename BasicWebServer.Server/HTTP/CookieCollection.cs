namespace BasicWebServer.Server.HTTP
{
    using System.Collections;
    using System.Collections.Generic;
    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> cookies;

        public CookieCollection()
            => cookies = new();

        public string this[string name]
            => cookies[name].Value;

        public void Add(string name, string value)
            => cookies[name] = new Cookie(name, value);
        
        public bool Contains(string name)
            => cookies.ContainsKey(name);

        public IEnumerator<Cookie> GetEnumerator()
            => this.cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}

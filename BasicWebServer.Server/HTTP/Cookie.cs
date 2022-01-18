namespace BasicWebServer.Server.HTTP
{
    using BasicWebServer.Server.Common;
    public class Cookie
    {
        public Cookie(string _name, string _value)
        {
            Guard.AgainstNull(_name, nameof(_name));
            Guard.AgainstNull(_value, nameof(_value));

            Name = _name;
            Value = _value;
        }

        public string Name { get; init; }

        public string Value { get; init; }

        public override string ToString()
            => $"{Name}={Value}";
    }
}

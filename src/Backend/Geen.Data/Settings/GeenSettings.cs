namespace Geen.Data.Settings
{
    public class GeenSettings
    {
        public virtual DatabaseSettings Database { get; set; }
        public virtual Authentication Authentication { get; set; }
        public virtual Prerender Prerender { get; set; }
    }

    public class DatabaseSettings
    {
        public string MongoUrl { get; set; }
        public string RedisUrl { get; set; }
    }

    public class Authentication
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class Prerender
    {
        public string StaticPath { get; set; }
    }
}

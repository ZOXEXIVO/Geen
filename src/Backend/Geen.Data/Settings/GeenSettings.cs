namespace Geen.Data.Settings;

public class GeenSettings
{
    public virtual DatabaseSettingsSettings Database { get; set; }
    public virtual AuthenticationSettings Authentication { get; set; }
    public virtual PrerenderSettings Prerender { get; set; }
    public virtual TracingSettings Tracing { get; set; }
}

public class DatabaseSettingsSettings
{
    public string MongoUrl { get; set; }
    public string RedisUrl { get; set; }
}

public class AuthenticationSettings
{
    public string Login { get; set; }
    public string Password { get; set; }
}

public class PrerenderSettings
{
    public string StaticPath { get; set; }
}

public class TracingSettings
{
    public JaegerSettings Jaeger { get; set; }

    public class JaegerSettings
    {
        public string Endpoint { get; set; }
    }
}
using Microsoft.Extensions.Configuration;

namespace Utils
{
    public static class CommonUtils
    {
        public static IConfiguration GetConfiguration(string basePath = null, string environment = null)
        {
            var env = environment ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            basePath ??= Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

        public static T GetConfigSection<T>(string sectionName, string basePath = null, string environment = null) where T : new()
        {
            var config = GetConfiguration(basePath, environment);
            var section = config.GetSection(sectionName);
            var settings = new T();
            section.Bind(settings);
            return settings;
        }
    }


}

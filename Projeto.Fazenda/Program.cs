using Projeto.Avaliacao.API;

public class Program
{

    public static void Main(string[] args)
    {
        GetConfiguration();
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    public static IConfigurationRoot GetConfiguration()
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        if (!string.IsNullOrWhiteSpace(envName))
        {
            builder.AddJsonFile($"appsettings.{envName}.json", optional: true);
        }

        builder.AddEnvironmentVariables();
        return builder.Build();
    }
}
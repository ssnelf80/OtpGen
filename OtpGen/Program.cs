using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace OtpGen;

public static class Program
{
    private const string CONSOLE_MODE = "-c";
    public static async Task Main(string[] args)
    {
        List<IOtpCodeProcessor> processors = new()
        {
            new OtpConsoleWriter()
        };

        if (args.Length > 2)
            processors.Add(new OtpFileWriter(args[1], args[2]));

        var asConsole = Debugger.IsAttached || args.Contains(CONSOLE_MODE);
        var builder = new HostBuilder()
            .ConfigureServices((_, services) => 
                services.AddHostedService(_ => 
                    new OtpGenerator(args[0], processors.ToArray())));

        await (asConsole ? builder.RunConsoleAsync() : builder.RunAsServiceAsync());
    }
    
    
}
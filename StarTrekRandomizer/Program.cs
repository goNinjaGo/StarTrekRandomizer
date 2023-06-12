using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace StarTrekRandomizer
{
    internal class Program
    {
        private static AppSettings AppSettings { get; set; } = null!;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            AppSettings = config.GetRequiredSection("settings").Get<AppSettings>()
                ?? throw new ArgumentNullException("No app settings detected");
            
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        static void RunOptions(Options opts)
        {
            var showFiles = GetShowList(opts);
            if(showFiles?.Count > 0)
            {
                var random = new Random();
                var episodePath = showFiles[random.Next(0, showFiles.Count)];
                
                using Process fileopener = new Process();
                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + episodePath + "\"";
                fileopener.Start();
            }
            else
            {
                HandleError("No episodes found.");
            }
        }

        static List<string>? GetShowList(Options opts) 
        {
            if (opts?.ShowTypes?.Count() > 0)
            {
                var shows = new List<Show>();
                foreach (var showType in opts.ShowTypes)
                {
                    shows.AddRange(AppSettings.Shows.Where(s => s.ShowType == showType));
                }

                return shows.SelectMany(s => Directory.GetFiles(AppSettings.Root + "\\" + s.Directory, "*", SearchOption.AllDirectories)).ToList();

            }
            else if (!string.IsNullOrEmpty(opts?.Show))
            {
                var showDirectory = AppSettings.Shows.Where(s => s.Directory == opts.Show).FirstOrDefault()?.Directory;
                if (string.IsNullOrEmpty(showDirectory))
                {
                    HandleError("Invalid show name.");
                }
                else
                {
                    return Directory.GetFiles(AppSettings.Root + "\\" + showDirectory, "*", SearchOption.AllDirectories).ToList();
                }
            }
            else
            {
                HandleError("Invalid options.");
            }
            return null;
        }

        static void HandleError(string message, int exitCode = 1)
        {
            Console.WriteLine(message);
            Environment.Exit(exitCode);
        }
    }
}
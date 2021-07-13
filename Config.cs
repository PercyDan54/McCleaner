using System.IO;
using Newtonsoft.Json;
using static System.Environment;

namespace ConsoleApp1
{
    [JsonObject]
    public class Config
    {
        public Config()
        {
            Dirs = new[] { "logs", "crash-reports", ".mixin.out" };
            McPath = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), ".minecraft");
        }

        public string McPath;
        public string[] Dirs;
    }
}

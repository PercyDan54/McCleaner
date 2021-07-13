using System;
using System.IO;
using Newtonsoft.Json;
using static System.Console;

namespace ConsoleApp1
{
    public class Program
    {
        public const string CONFIG_FILE = "config.json";
        private static Config config;

        public static void Main(string[] args)
        {
            Title = "清理";
            loadConfig();
            var mcDir = config.McPath;

            try
            {
                foreach (var dir in config.Dirs)
                {
                    deleteDir(Path.Combine(mcDir, dir));

                    // 版本隔离
                    var versionPath = Path.Combine(mcDir, "versions");

                    if (!Directory.Exists(versionPath))
                    {
                        throw new FileNotFoundException(null, versionPath);
                    }

                    foreach (var directory in Directory.GetDirectories(versionPath))
                    {
                        deleteDir(Path.Combine(directory, dir));
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message, ConsoleColor.DarkRed);
            }

            WriteLine("按任意键退出...");
            ReadKey();
        }

        private static void loadConfig()
        {
            try
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(CONFIG_FILE));
            }
            catch
            {
                WriteLine("读取配置文件失败，正在创建默认配置", ConsoleColor.DarkRed);
                createDefault();
            }
        }

        private static void createDefault()
        {
            config = new Config();
            if (File.Exists(CONFIG_FILE)) File.Delete(CONFIG_FILE);
            File.WriteAllText(CONFIG_FILE, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        private static void deleteDir(string dir, bool recursive = true)
        {
            try
            {
                if (!Directory.Exists(dir)) return;

                Directory.Delete(dir, recursive);
                WriteLine($"正在删除 {dir}");
            }
            catch (Exception e)
            {
                WriteLine(e.Message, ConsoleColor.DarkRed);
            }
        }

        public static void WriteLine(string msg, ConsoleColor color = ConsoleColor.White)
        {
            var currentColor = ForegroundColor;
            ForegroundColor = color;
            Console.WriteLine(msg);
            ForegroundColor = currentColor;
        }
    }
}

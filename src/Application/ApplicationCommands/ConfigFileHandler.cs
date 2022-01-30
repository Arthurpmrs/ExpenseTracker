using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.ApplicationCommands
{
    public class ConfigFileHandler
    {
        private string ConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ExpenseTracker",
            "config.json"
            );
        public static string GetDBName()
        {
            ConfigFileHandler handler = new ConfigFileHandler();
            ConfigInfo config = handler.LoadOrCreateConfigFile();
            return config.DBName;
        }

        private string PromptForDBName()
        {
            Console.WriteLine("Enter DB Name:");
            string dbname = Console.ReadLine();
            if (dbname == "")
            {
                return "expensetracker";
            } else
            {
                return dbname;
            }
        }
        private ConfigInfo LoadOrCreateConfigFile()
        {
            if (!File.Exists(this.ConfigFilePath))
            {
                string dbname = PromptForDBName();
                ConfigInfo initConfig = new ConfigInfo()
                {
                    DBName = dbname,
                    DBPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dbname)
                };
                string json = JsonSerializer.Serialize(initConfig);
                File.WriteAllText(ConfigFilePath, json);
                return initConfig;
            } else
            {
                using (StreamReader r = new StreamReader(ConfigFilePath))
                {
                    string jsonStr = r.ReadToEnd();
                    ConfigInfo config = JsonSerializer.Deserialize<ConfigInfo>(jsonStr);
                    return config;
                }
            }

        }
    }
}

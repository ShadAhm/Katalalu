using System;
using System.IO;
using Newtonsoft.Json;
using ShadAhm.Katalalu.App.Models;
using Cons = ShadAhm.Katalalu.App.Services.ConsoleService;

namespace ShadAhm.Katalalu.App.Services
{
    public sealed class SettingService
    {
        private static SettingService instance = null;
        private static readonly object padlock = new object();

        SettingService() {}

        public static SettingService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SettingService();
                        }
                    }
                }
                return instance;
            }
        }

        public string GetCsvLocation()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userSettings.json");
            string json = File.ReadAllText(fileName);

            UserSetting userSetting = JsonConvert.DeserializeObject<UserSetting>(json);

            return userSetting.CsvLocation;
        }

        public void Init(string csvLocation)
        {
            var userSetting = new UserSetting { CsvLocation = csvLocation };
            var json = JsonConvert.SerializeObject(userSetting, Formatting.Indented);

            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userSettings.json");
            File.WriteAllText(fileName, json);

            Cons.WriteLine("Initialized done.");
            Cons.WriteLine($"User settings stored at: {fileName}");
        }
    }
}
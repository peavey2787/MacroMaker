namespace MacroMaker.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using Newtonsoft.Json;

    internal static class AppSettings
    {
        private static readonly SemaphoreSlim FileLock = new SemaphoreSlim(1, 1);

        public static void Save<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }

            lock (FileLock)
            {
                var settings = LoadSettings();
                settings[key] = JsonConvert.SerializeObject(data, Formatting.Indented);
                SaveSettings(settings);
            }
        }

        public static T Load<T>(string key) where T : class, new()
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }

            lock (FileLock)
            {
                var settings = LoadSettings();
                if (settings.TryGetValue(key, out var serializedValue))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(serializedValue);
                    }
                    catch (JsonException ex)
                    {
                        throw new InvalidOperationException($"Error deserializing '{key}' from AppSettings: {ex.Message}", ex);
                    }
                }
                else
                {
                    return null;
                }
            }
        }


        private static void SaveSettings(Dictionary<string, string> settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(GetSettingsFilePath(), json);
        }

        private static Dictionary<string, string> LoadSettings()
        {
            if (File.Exists(GetSettingsFilePath()))
            {
                string json = File.ReadAllText(GetSettingsFilePath());
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        private static string GetSettingsFilePath() => "AppSettings.json";
    }


}

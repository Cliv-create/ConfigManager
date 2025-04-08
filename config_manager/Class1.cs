using System.Text.Json.Serialization;
using System.Text.Json;

namespace config_manager
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Dictionary<string, string>))]
    internal partial class ConfigJsonContext : JsonSerializerContext
    {
    }

    internal static class ConfigManager
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static Dictionary<string, string>? _config;

        /// <summary>
        /// Loads config.json from folder, containing executable. config.json gets deserialized in Dictionary<string, string>. To get config values use Get property.
        /// </summary>
        /// <exception cref="Exception">Exception if config.json was not found.</exception>
        public static void LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                string json = File.ReadAllText(ConfigFilePath);
                _config = JsonSerializer.Deserialize(json, ConfigJsonContext.Default.DictionaryStringString);
            }
            else
            {
                throw new Exception("config.json not found!");
            }
        }

        /// <summary>
        /// Generates config.json in folder, containing executable. Config will have all the settings field. Mandatory settings: token, yt-dlp_path.
        /// </summary>
        public static void GenerateInitialConfig()
        {
            _config = new Dictionary<string, string>();
            _config.Add("key", "value");
            string json = JsonSerializer.Serialize(_config, ConfigJsonContext.Default.DictionaryStringString);
            File.WriteAllText(ConfigFilePath, json);
        }

        /// <summary>
        /// Uses TryGetValue to get value from config.json. Can generate exception.
        /// </summary>
        /// <param name="key">Dictionary key.</param>
        /// <param name="defaultValue">null by default. If set, will return this value if nothning was found.</param>
        /// <returns>Found value, or null (defaultValue if set) if nothning was found.</returns>
        public static string Get(string key, string defaultValue = null)
        {
            return _config.TryGetValue(key, out string value) ? value : defaultValue;
        }

        // Config is meant to be defined by user manually.
        // It is not meant to be changed in runtime, but to be changed manually.
        // If runtime config change capabilities are needed - uncomment this code.

        /*
        public static void SaveConfig()
        {
            string json = JsonSerializer.Serialize(_config, ConfigJsonContext.Default.DictionaryStringString);
            File.WriteAllText(ConfigFilePath, json);
        }
        */

        /*
        public static void Set(string key, string value)
        {
            _config[key] = value;
            SaveConfig();
        }
        */
    }
}

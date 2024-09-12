using Newtonsoft.Json;
using Visma_task.Constants;
using Visma_task.Data;

namespace Visma_task.Helpers
{
    public class ShortageJsonHelper
    {
        public static bool SaveShortages(Dictionary<string, Shortage> shortages)
        {
            try
            {
                string json = JsonConvert.SerializeObject(shortages, Formatting.Indented);
                File.WriteAllText(Paths.ShortageFilePath, json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving shortages: {ex.Message}");
            }

            return false;
        }

        public static bool AddShortage(Shortage shortage)
        {
            try
            {
                var shortages = ReadShortages();

                string key = GenerateKey(shortage);

                if (shortages.ContainsKey(key) && shortage.Priority < shortages[key].Priority)
                {
                    return false;
                }

                shortages[key] = shortage;
                SaveShortages(shortages);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new shortage: {ex.Message}");
            }

            return false;
        }

        public static bool RemoveShortage(Shortage shortage)
        {
            try
            {
                var shortages = ReadShortages();

                string key = GenerateKey(shortage);

                if (shortages.ContainsKey(key))
                {
                    shortages.Remove(key);
                    SaveShortages(shortages);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing shortage: {ex.Message}");
            }

            return false;
        }

        public static Dictionary<string, Shortage> ReadShortages()
        {
            if (!File.Exists(Paths.ShortageFilePath))
            {
                return new Dictionary<string, Shortage>();
            }

            string json = File.ReadAllText(Paths.ShortageFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, Shortage>>(json) ?? new Dictionary<string, Shortage>();
        }

        public static string GenerateKey(Shortage shortage)
        {
            return $"{shortage.Title}-{shortage.Room}";
        }
    }
}

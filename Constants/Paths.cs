namespace Visma_task.Constants
{
    public class Paths
    {
        public static string DirectoryPath => AppDomain.CurrentDomain.BaseDirectory;
        public static string WritePath => Path.Combine(DirectoryPath, "WriteData");
        public static string ShortageFilePath => Path.Combine(WritePath, "Shortages.json");
    }
}

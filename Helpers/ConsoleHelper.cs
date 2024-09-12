namespace Visma_task.Helpers
{
    public class ConsoleHelper
    {
        public static void WriteToConsole(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
        }

        public static char GetCharInputFromConsole(string text)
        {
            WriteToConsole(text);
            return Console.ReadKey().KeyChar;
        }

        public static string GetStringInputFromConsole(string text)
        {
            WriteToConsole(text);
            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "";
            }

            return userInput;
        }
    }
}

namespace Visma_task.Constants
{
    public class Messages
    {
        public static string Continue => "Press any key to continue...";
        public static string SuccessContinue => "Success!\n" + Continue;
        public static string IncorrectValueContinue => "Incorrect value!\n" + Continue;

        public static string GetFilterMessage(string filterColumn) => $"Type in {filterColumn} value (leave blank if filter parameter is not needed):";
    }
}

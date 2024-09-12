using System.Globalization;
using Visma_task.Constants;
using Visma_task.Data;
using Visma_task.Enums;
using Visma_task.Handlers;
using Visma_task.Helpers;


RunApp();

void RunApp()
{
    if (!Directory.Exists(Paths.WritePath))
    {
        Directory.CreateDirectory(Paths.WritePath);
    }

    LoginHandler login;
    char input;
    bool isRunning = true;

    input = ConsoleHelper.GetCharInputFromConsole("Are you an admin?\nYes - Y;\nNo - N");

    if (input == 'Y' || input == 'y')
    {
        login = LoginHandler.Login(true);
    }
    else
    {
        login = LoginHandler.Login(false);
    }

    while(isRunning)
    {
        input = ConsoleHelper.GetCharInputFromConsole("1. Add shortage.\n2. Remove shortage.\n3. List all shortages\n4. Filtered search\nX - leave application");

        switch (input)
        {
            case '1':
                if (AddShortage())
                {
                    ConsoleHelper.GetCharInputFromConsole(Messages.SuccessContinue);
                }
                else
                {
                    ConsoleHelper.GetCharInputFromConsole(Messages.IncorrectValueContinue);
                }
                break;
            case '2':
                if (RemoveShortage(login.IsAdmin))
                {
                    ConsoleHelper.GetCharInputFromConsole(Messages.SuccessContinue);
                }
                else 
                {
                    ConsoleHelper.GetCharInputFromConsole("Removing has failed...\n" + Messages.Continue);
                }
                break;
            case '3':
                PrintShortages(login.IsAdmin);
                Console.WriteLine(Messages.Continue);
                Console.ReadKey();
                break;
            case '4':
                FilteredSearch(login.IsAdmin);
                break;
            case 'X':
            case 'x':
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }
}

bool AddShortage()
{
    var title = ConsoleHelper.GetStringInputFromConsole("Title: ");
    var name = ConsoleHelper.GetStringInputFromConsole("Name: ");

    var roomInput = ConsoleHelper.GetStringInputFromConsole("Room:\n 0 - Meeting room\n 1 - Kitchen\n 2 - Bathroom");
    Room room;
    if (!Enum.TryParse(roomInput, out room))
    {
        return false;
    }

    var categoryInput = ConsoleHelper.GetStringInputFromConsole("Room:\n 0 - Electronics\n 1 - Food\n 2 - Other");
    Category category;
    if (!Enum.TryParse(categoryInput, out category))
    {
        return false;
    }

    int priority;
    if (!int.TryParse(ConsoleHelper.GetStringInputFromConsole("Priority (1-10): "), out priority))
    {
        return false;
    }

    return ShortageJsonHelper.AddShortage(
            new Shortage(title, name, room, category, priority)
        );
}

void PrintShortages(bool isAdmin)
{
    Console.Clear();
    var shortages = ShortageJsonHelper.ReadShortages();

    if (shortages.Count == 0)
    {
        ConsoleHelper.WriteToConsole("No shortages found.");
        return;
    }

    foreach (var entry in shortages)
    {
        PrintShortage(entry.Key, entry.Value, isAdmin);
    }
}

bool RemoveShortage(bool isAdmin)
{
    var title = ConsoleHelper.GetStringInputFromConsole("Enter Title of shortage to delete: ");
    var roomInput = ConsoleHelper.GetStringInputFromConsole("Choose room of shortage to delete:\n 0 - Meeting room\n 1 - Kitchen\n 2 - Bathroom");

    Room room;
    if (!Enum.TryParse(roomInput, out room))
    {
        return false;
    }

    var key = $"{title}-{room}";

    var shortages = ShortageJsonHelper.ReadShortages();
    if (shortages[key].CreatedBy == Environment.UserName || isAdmin)
    {
        return ShortageJsonHelper.RemoveShortage(shortages[key]);
    }

    return false;
}

void PrintShortage(string key, Shortage shortage, bool isAdmin)
{
    if (shortage.CreatedBy == Environment.UserName || isAdmin)
    {
        Console.WriteLine($"Key: {key}");
        Console.WriteLine($"Title: {shortage.Title}");
        Console.WriteLine($"Name: {shortage.Name}");
        Console.WriteLine($"Room: {shortage.Room}");
        Console.WriteLine($"Category: {shortage.Category}");
        Console.WriteLine($"Priority: {shortage.Priority}");
        Console.WriteLine($"Created On: {shortage.CreatedOn}");
        Console.WriteLine();
    }
}

bool FilteredSearch(bool isAdmin)
{
    string format = "yyyy-MM-dd";

    var titleFilter = ConsoleHelper.GetStringInputFromConsole(Messages.GetFilterMessage("Title"));
    var fromDateInput = ConsoleHelper.GetStringInputFromConsole(Messages.GetFilterMessage("From Date " + format));
    var toDateInput = ConsoleHelper.GetStringInputFromConsole(Messages.GetFilterMessage("To Date " + format));
    var categoryValue = ConsoleHelper.GetStringInputFromConsole(Messages.GetFilterMessage("Room(0, 1, 2, ..)"));
    var roomValue = ConsoleHelper.GetStringInputFromConsole(Messages.GetFilterMessage("Category(0, 1, 2, ..)"));

    DateTime? fromDate = null;
    DateTime? toDate = null;

    if (DateTime.TryParseExact(fromDateInput, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFromDate))
    {
        fromDate = parsedFromDate;
    }
    if (DateTime.TryParseExact(toDateInput, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedToDate))
    {
        toDate = parsedToDate;
    }

    var shortages = ShortageJsonHelper.ReadShortages();

    List<Shortage> filteredShortages = shortages.Values.ToList();

    if (!string.IsNullOrEmpty(titleFilter))
    {
        filteredShortages = filteredShortages
        .Where(s => s.Title.ToLower().Contains(titleFilter.ToLower()))
        .ToList();
    }

    if (fromDate != null && toDate != null)
    {
        filteredShortages = filteredShortages
        .Where(s => s.CreatedOn >= fromDate.Value &&
                    s.CreatedOn <= toDate.Value)
        .ToList();
    }

    if (!string.IsNullOrEmpty(roomValue))
    {
        Room room;
        if (!Enum.TryParse(roomValue, out room))
        {
            return false;
        }

        filteredShortages = filteredShortages
        .Where(s => s.Room == room)
        .ToList();
    }

    if (!string.IsNullOrEmpty(categoryValue))
    {
        Category category;
        if (!Enum.TryParse(categoryValue, out category))
        {
            return false;
        }

        filteredShortages = filteredShortages
        .Where(s => s.Category == category)
        .ToList();
    }

    filteredShortages = filteredShortages
        .OrderByDescending(s => s.Priority)
        .ToList();

    foreach (var shortage in filteredShortages)
    {
        var key = $"{shortage.Title}-{shortage.Room}";
        PrintShortage(key, shortage, isAdmin);
    }

    Console.WriteLine(Messages.Continue);
    Console.ReadKey();

    return filteredShortages.Any();
}

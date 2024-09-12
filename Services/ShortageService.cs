using Visma_task.Data;
using Visma_task.Enums;
using Visma_task.Helpers;

namespace UnitTests.Services
{
    public class ShortageService
    {
        public ShortageService()
        {
        }

        public bool AddShortage(string title, Room room)
        {
            var name = "TestName";
            var priority = 5;

            var shortage = new Shortage(title, name, room, Category.Other, priority);

            return ShortageJsonHelper.AddShortage(shortage);
        }

        public bool RemoveShortage(string title, Room room, bool isAdmin)
        {
            var key = $"{title}-{room}";

            var shortages = ShortageJsonHelper.ReadShortages();

            if (shortages.TryGetValue(key, out var shortage))
            {
                if (shortage.CreatedBy == Environment.UserName || isAdmin)
                {
                    return ShortageJsonHelper.RemoveShortage(shortage);
                }
            }

            return false;
        }

        public void GetShortages() => ShortageJsonHelper.ReadShortages();
    }
}

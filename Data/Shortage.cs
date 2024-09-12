using Visma_task.Enums;

namespace Visma_task.Data
{
    public class Shortage
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public Room Room { get; set; }
        public Category Category { get; set; }

        private int _priority;
        public int Priority
        {
            get => _priority;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentOutOfRangeException("Priority must be between 1 and 10.");
                _priority = value;
            }
        }

        public DateTime CreatedOn { get; private set; }
        public string CreatedBy { get; private set; }

        public Shortage(string title, string name, Room room, Category category, int priority)
        {
            Title = title;
            Name = name;
            Room = room;
            Category = category;
            Priority = priority;
            CreatedOn = DateTime.Now;
            CreatedBy = Environment.UserName;
        }
    }
}

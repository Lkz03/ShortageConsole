namespace Visma_task.Handlers
{
    public class LoginHandler
    {
        public bool IsAdmin { get; private set; }

        public static LoginHandler Login(bool isAdmin) => new LoginHandler(isAdmin);

        private LoginHandler(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }
    }
}

using SQLite;

namespace TodoApp
{
    public partial class App : Application
    {
        public App()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "tasks.db3");
            var _database = new SQLiteAsyncConnection(dbPath);
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
using Microsoft.VisualBasic.FileIO;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TodoList.Models.Models;

namespace TodoList.Models.Services
{
    public class DatabaseHelper
    {
        static SQLiteAsyncConnection _database;

        public static async Task InitializeAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TareasApp.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<TaskItem>();
        }

        public static Task<List<TaskItem>> GetTasksAsync()
        {
            return _database.Table<TaskItem>().ToListAsync();
        }

        public static Task<TaskItem> GetTaskAsync(int id)
        {
            return _database.Table<TaskItem>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public static Task<int> SaveTaskAsync(TaskItem task)
        {
            if (task.Id != 0)
            {
                return _database.UpdateAsync(task);
            }
            else
            {
                return _database.InsertAsync(task);
            }
        }

        public static Task<int> DeleteTaskAsync(TaskItem task)
        {
            return _database.DeleteAsync(task);
        }
    }
}
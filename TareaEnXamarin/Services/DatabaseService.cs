using SQLite;
using TodoApp.Models;

namespace TodoApp.Services
{
    /// <summary>
    /// Handles all database operations for our Todo app
    /// This service abstracts SQLite complexity from the rest of the app
    /// </summary>
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        /// <summary>
        /// Initialize database connection and create tables
        /// This happens when the service is first used
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            if (_database != null)
                return; // Already initialized

            // Get the path where we'll store our database file
            // This creates a local SQLite file that persists between app sessions
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "TodoDatabase.db");

            // Create connection with full-text search support
            _database = new SQLiteAsyncConnection(databasePath);

            // Create the TodoItems table if it doesn't exist
            // SQLite will automatically handle the table structure based on our model
            await _database.CreateTableAsync<TodoItem>();
        }

        /// <summary>
        /// Retrieve all todo items, ordered by creation date (newest first)
        /// This method shows how to perform queries with SQLite
        /// </summary>
        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            await InitializeDatabaseAsync();

            // Order by priority first (high priority = 1), then by creation date
            return await _database.Table<TodoItem>()
                .OrderBy(x => x.Priority)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific todo item by its ID
        /// Used when editing an existing todo
        /// </summary>
        public async Task<TodoItem> GetTodoItemAsync(int id)
        {
            await InitializeDatabaseAsync();
            return await _database.Table<TodoItem>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Save a new todo item to the database
        /// SQLite will automatically assign an ID
        /// </summary>
        public async Task<int> SaveTodoItemAsync(TodoItem item)
        {
            await InitializeDatabaseAsync();

            if (item.Id != 0)
            {
                // Update existing item
                var result = await _database.UpdateAsync(item);
                System.Diagnostics.Debug.WriteLine($"Update Result: {result}"); // Add this line
                return result;
            }
            else
            {
                // Insert new item
                return await _database.InsertAsync(item);
            }
        }

        /// <summary>
        /// Remove a todo item from the database
        /// This is a permanent deletion
        /// </summary>
        public async Task<int> DeleteTodoItemAsync(TodoItem item)
        {
            await InitializeDatabaseAsync();
            return await _database.DeleteAsync(item);
        }

        /// <summary>
        /// Search todos by title or description
        /// Demonstrates SQLite's text search capabilities
        /// </summary>
        public async Task<List<TodoItem>> SearchTodosAsync(string searchTerm)
        {
            await InitializeDatabaseAsync();

            return await _database.Table<TodoItem>()
                .Where(x => x.Title.Contains(searchTerm) || x.Description.Contains(searchTerm))
                .OrderBy(x => x.Priority)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
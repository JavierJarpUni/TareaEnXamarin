using SQLite;

namespace TodoApp.Models
{
    /// <summary>
    /// Represents a single todo item in our database
    /// The attributes here tell SQLite how to structure our table
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// Primary key that auto-increments for each new todo
        /// This ensures each todo has a unique identifier
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// The main text content of the todo item
        /// NotNull ensures we always have a title
        /// </summary>
        [NotNull]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Optional detailed description of the task
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Tracks whether the user has completed this task
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// When this todo was first created
        /// Helps with sorting and organization
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Optional due date for time-sensitive tasks
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Priority level: 1 = High, 2 = Medium, 3 = Low
        /// Default to medium priority
        /// </summary>
        public int Priority { get; set; } = 2;
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel for adding new todos or editing existing ones
    /// This demonstrates how to handle both create and update operations
    /// QueryProperty allows us to receive the TodoId parameter from navigation
    /// </summary>
    [QueryProperty(nameof(TodoId), "TodoId")]
    public partial class AddEditTodoViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        /// <summary>
        /// The ID of the todo being edited (0 for new todos)
        /// </summary>
        [ObservableProperty]
        private int todoId;

        /// <summary>
        /// The title/name of the todo item
        /// </summary>
        [ObservableProperty]
        private string title = string.Empty;

        /// <summary>
        /// Detailed description of the todo
        /// </summary>
        [ObservableProperty]
        private string description = string.Empty;

        /// <summary>
        /// Whether this todo is completed
        /// </summary>
        [ObservableProperty]
        private bool isCompleted;

        /// <summary>
        /// Optional due date for the todo
        /// </summary>
        [ObservableProperty]
        private DateTime? dueDate;

        /// <summary>
        /// Priority level of the todo
        /// </summary>
        [ObservableProperty]
        private int priority = 2; // Default to medium priority

        /// <summary>
        /// Loading state for database operations
        /// </summary>
        [ObservableProperty]
        private bool isLoading;

        /// <summary>
        /// Page title that changes based on whether we're adding or editing
        /// </summary>
        [ObservableProperty]
        private string pageTitle = "Add Todo";

        public AddEditTodoViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Called when the TodoId property changes (from navigation)
        /// This loads existing todo data if we're editing
        /// </summary>
        partial void OnTodoIdChanged(int value)
        {
            if (value > 0)
            {
                // We're editing an existing todo
                PageTitle = "Edit Todo";
                LoadTodoAsync(value);
            }
            else
            {
                // We're creating a new todo
                PageTitle = "Add Todo";
                ResetForm();
            }
        }

        /// <summary>
        /// Load existing todo data for editing
        /// Shows how to populate form fields from database
        /// </summary>
        private async Task LoadTodoAsync(int id)
        {
            try
            {
                IsLoading = true;

                var todo = await _databaseService.GetTodoItemAsync(id);
                if (todo != null)
                {
                    // Populate form fields with existing data
                    Title = todo.Title;
                    Description = todo.Description;
                    IsCompleted = todo.IsCompleted;
                    DueDate = todo.DueDate;
                    Priority = todo.Priority;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to load todo: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Reset form to default values for new todo creation
        /// </summary>
        private void ResetForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            IsCompleted = false;
            DueDate = null;
            Priority = 2; // Medium priority
        }

        /// <summary>
        /// Save the todo (create new or update existing)
        /// This method demonstrates form validation and data persistence
        /// </summary>
        [RelayCommand]
        private async Task SaveTodoAsync()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(Title))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error",
                    "Please enter a title for your todo.", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                // Create todo object with form data
                var todo = new TodoItem
                {
                    Id = TodoId, // 0 for new, existing ID for updates
                    Title = Title.Trim(),
                    Description = Description?.Trim() ?? string.Empty,
                    IsCompleted = IsCompleted,
                    DueDate = DueDate,
                    Priority = Priority
                };

                // Save to database
                await _databaseService.SaveTodoItemAsync(todo);

                await Task.Delay(100); // Add a small delay (100 milliseconds)

                // Navigate back to main page
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to save todo: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Cancel editing and return to main page
        /// </summary>
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Clear the due date
        /// Provides easy way to remove due date without date picker complexity
        /// </summary>
        [RelayCommand]
        private void ClearDueDate()
        {
            DueDate = null;
        }
    }
}
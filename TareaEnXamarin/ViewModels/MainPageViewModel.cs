using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Views;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel for the main todo list screen
    /// Handles the presentation logic and user interactions
    /// ObservableObject provides property change notifications for data binding
    /// </summary>
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        /// <summary>
        /// Collection of todos that automatically updates the UI when changed
        /// ObservableCollection notifies the UI about add/remove operations
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<TodoItem> todos;

        /// <summary>
        /// Search text for filtering todos
        /// When this changes, the UI will automatically update
        /// </summary>
        [ObservableProperty]
        private string searchText = string.Empty;

        /// <summary>
        /// Shows loading indicator during database operations
        /// </summary>
        [ObservableProperty]
        private bool isLoading;

        /// <summary>
        /// Title for a new todo being added
        /// </summary>
        [ObservableProperty]
        private string newTodoTitle;

        public MainPageViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Todos = new ObservableCollection<TodoItem>();
        }

        /// <summary>
        /// Load todos from database when the screen appears
        /// This method demonstrates async operations in MVVM
        /// </summary>
        [RelayCommand]
        private async Task LoadTodosAsync()
        {
            try
            {
                IsLoading = true;
                var todoItems = await _databaseService.GetTodoItemsAsync();
                Todos.Clear();
                foreach (var todo in todoItems)
                {
                    Todos.Add(todo);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to load todos: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Navigate to edit screen for a specific todo
        /// Shows how to pass parameters between pages
        /// </summary>
        [RelayCommand]
        private async Task EditTodoAsync(TodoItem todo)
        {
            if (todo == null) return;
            await Shell.Current.GoToAsync($"///AddEditTodoPage?TodoId={todo.Id}");
        }

        /// <summary>
        /// Toggle the completion status of a todo
        /// This provides quick interaction without opening edit screen
        /// </summary>
        [RelayCommand]
        private async Task ToggleCompleteAsync(TodoItem todo)
        {
            try
            {
                todo.IsCompleted = !todo.IsCompleted;
                await _databaseService.SaveTodoItemAsync(todo);
                // No need to fully reload, just update the local collection if needed for immediate UI feedback
                // For simplicity, we can reload.
                await LoadTodosAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to update todo: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Delete a todo item with confirmation
        /// Shows user-friendly interaction patterns
        /// </summary>
        [RelayCommand]
        private async Task DeleteTodoAsync(TodoItem todo)
        {
            if (todo == null) return;

            var result = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete '{todo.Title}'?",
                "Delete", "Cancel");

            if (result)
            {
                try
                {
                    await _databaseService.DeleteTodoItemAsync(todo);
                    await LoadTodosAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        $"Failed to delete todo: {ex.Message}", "OK");
                }
            }
        }

        /// <summary>
        /// Search todos based on user input
        /// Demonstrates real-time filtering
        /// </summary>
        [RelayCommand]
        private async Task SearchTodosAsync()
        {
            try
            {
                IsLoading = true;

                List<TodoItem> todoItems;

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    // If search is empty, show all todos
                    todoItems = await _databaseService.GetTodoItemsAsync();
                }
                else
                {
                    // Search by title and description
                    todoItems = await _databaseService.SearchTodosAsync(SearchText);
                }

                Todos.Clear();
                foreach (var todo in todoItems)
                {
                    Todos.Add(todo);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Search failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Command to add a new todo from the input field
        /// </summary>
        [RelayCommand]
        private async Task AddNewTodoAsync()
        {
            if (!string.IsNullOrWhiteSpace(NewTodoTitle))
            {
                IsLoading = true;
                var newTodo = new TodoItem { Title = NewTodoTitle };
                await _databaseService.SaveTodoItemAsync(newTodo);
                NewTodoTitle = string.Empty; // Clear the input field
                await LoadTodosAsync();
                IsLoading = false;
            }
        }
    }
}
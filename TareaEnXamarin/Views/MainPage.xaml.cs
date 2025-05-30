using TodoApp.ViewModels;

namespace TodoApp.Views
{
    /// <summary>
    /// Code-behind for the main page
    /// In MVVM, we keep this minimal and handle logic in the ViewModel
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        /// <summary>
        /// Load todos when the page appears
        /// This ensures we always show fresh data
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainPageViewModel viewModel)
            {
                await viewModel.LoadTodosCommand.ExecuteAsync(null);
            }
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox && BindingContext is ViewModels.MainPageViewModel viewModel && checkBox.BindingContext is Models.TodoItem todo)
            {
                todo.IsCompleted = e.Value;
                await viewModel.ToggleCompleteCommand.ExecuteAsync(todo);
            }
        }
    }
}
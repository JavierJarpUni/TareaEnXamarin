using TodoApp.ViewModels;

namespace TodoApp.Views
{
    public partial class AddEditTodoPage : ContentPage
    {
        public AddEditTodoPage(AddEditTodoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
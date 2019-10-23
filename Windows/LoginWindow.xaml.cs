using System.Windows;
using EmployeeRegistry.ViewModels;

namespace EmployeeRegistry
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(CloseShowWindow);
        }

        private void CloseShowWindow(Window window)
        {
            Hide();
            window.Show();
            Close();
        }
    }
}

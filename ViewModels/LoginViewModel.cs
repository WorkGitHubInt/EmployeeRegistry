using EmployeeRegistry.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeRegistry.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Commands
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Properties
        public string Login { get; set; }
        public string Password { get; set; }
        #endregion

        public delegate void CloseWindow(Window window);
        private CloseWindow CloseShow;

        public LoginViewModel(CloseWindow closeWindow)
        {
            SQLDatabase.LoadData();
            LoginCommand = new RelayParamCommand((parameter) => LoginAccount(parameter));
            CloseShow = closeWindow;
        }

        private void LoginAccount(object parameter)
        {
            Password = (parameter as PasswordBox).Password;
            if (Login.Equals("admin"))
            {
                if (Password.Equals("admin"))
                {
                    var window = new SuperUserWindow();
                    CloseShow(window);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Employee employee = SQLDatabase.LoginQuery(Login);
                if (employee.Password == Password)
                {
                    var vm = new EmployeeViewModel(employee);
                    var window = new EmployeeWindow
                    {
                        DataContext = vm
                    };
                    CloseShow(window);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}

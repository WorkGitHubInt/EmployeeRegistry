using System.Windows;
using EmployeeRegistry.ViewModels;

namespace EmployeeRegistry
{
    public partial class SuperUserWindow : Window
    {
        public SuperUserWindow()
        {
            InitializeComponent();
            DataContext = new SuperUserViewModel();
        }
    }
}

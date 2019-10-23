using EmployeeRegistry.ViewModels;
using System.Windows;

namespace EmployeeRegistry
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
            DataContext = new AddEmployeeViewModel
            {
                ConfirmAction = new System.Action(() => { DialogResult = true; })
            };
        }
    }
}

using System;
using System.Windows;
using System.Windows.Input;
using EmployeeRegistry.Models;

namespace EmployeeRegistry.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        #region Commands
        public ICommand RequestSelfSalaryCommand { get; set; }
        public ICommand RequestSubSalaryCommand { get; set; }
        #endregion

        #region Properties
        public Employee Employee { get; set; }
        public Employee SelectedSub { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        #endregion

        public EmployeeViewModel(Employee employee)
        {
            RequestSelfSalaryCommand = new RelayCommand(() => RequestSelfSalary());
            RequestSubSalaryCommand = new RelayCommand(() => RequestSubSalary());
            Employee = employee;
        }

        private void RequestSubSalary()
        {
            if (SelectedSub != null)
            {
                MessageBox.Show($"Зарплата сотрудника {SelectedSub.Name}: {SelectedSub.GetSalary(SelectedDate)} руб.");
            } else
            {
                MessageBox.Show("Не выбран подчиненный!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RequestSelfSalary()
        {
            MessageBox.Show($"Ваша зарплата: {Employee.GetSalary(SelectedDate)} руб.");
        }
    }
}

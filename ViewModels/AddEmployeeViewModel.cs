using EmployeeRegistry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EmployeeRegistry.ViewModels
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        #region Commands
        public ICommand AddCommand { get; set; }
        #endregion

        #region Properties
        public Employee Employee { get; set; } = new Employee();
        public List<Employee> Employees { get; set; }
        public List<Position> Positions { get; set; }
        public Employee SelectedEmployee { get; set; }
        public Position SelectedPosition { get; set; }
        #endregion

        public Action ConfirmAction { get; set; }

        public AddEmployeeViewModel()
        {
            AddCommand = new RelayCommand(() => AddEmployee());
            Employees = SQLDatabase.Employees.Where(e => e.Position.Id != 1).ToList();
            Positions = SQLDatabase.Positions;
        }

        private void AddEmployee() //Тут должна быть куча проверок на правильность заполнения полей
        {
            if (SelectedPosition != null)
            {
                Employee.Position = SelectedPosition;
                Employee.Chief = SelectedEmployee;
                SQLDatabase.AddEmployee(Employee);
                ConfirmAction();
            } else
            {
                MessageBox.Show("Не выбрана должность!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

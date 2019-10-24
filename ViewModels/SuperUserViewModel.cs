using EmployeeRegistry.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EmployeeRegistry.ViewModels
{
    public class SuperUserViewModel : BaseViewModel
    {
        #region Commands
        public ICommand Query1Command { get; set; }
        public ICommand Query2Command { get; set; }
        public ICommand Query3Command { get; set; }
        public ICommand RequestSalaryCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand DisplaySalaryCommand { get; set; }
        #endregion

        #region Properties
        public DataView GridView { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        public Employee SelectedEmployee { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        #endregion Properties

        public SuperUserViewModel()
        {
            Query1Command = new RelayCommand(() => Query1());
            Query2Command = new RelayCommand(() => Query2());
            Query3Command = new RelayCommand(() => Query3());
            RequestSalaryCommand = new RelayCommand(() => RequestSalary());
            AddEmployeeCommand = new RelayCommand(() => AddEmployee());
            DisplaySalaryCommand = new RelayCommand(() => DisplaySalary());
            Employees = SQLDatabase.Employees.ToObservableCollection();
        }

        private void RequestSalary()
        {
            if (SelectedEmployee != null)
            {
                MessageBox.Show($"Зарплата сотрудника {SelectedEmployee.Name}: {SelectedEmployee.GetSalary(SelectedDate)} руб.", "", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                MessageBox.Show("Не выбран сотрудник!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddEmployee()
        {
            var window = new AddEmployeeWindow();
            if (window.ShowDialog() == true)
            {
                SQLDatabase.LoadData();
                Employees = SQLDatabase.Employees.ToObservableCollection();
            }
        }

        private void DisplaySalary()
        {
            MessageBox.Show($"Зарплата всех сотрудников: {Employees.Sum(e => e.GetSalary(SelectedDate))} руб.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Query1()
        {
            var dv = new DataView(SQLDatabase.ExecuteQuery(Properties.Resources.Query1).Tables[0]);
            dv.Table.Columns[1].ColumnName = GetMonthName(DateTime.Now.AddMonths(-1));
            dv.Table.Columns[2].ColumnName = GetMonthName(DateTime.Now.AddMonths(-2));
            dv.Table.Columns[3].ColumnName = GetMonthName(DateTime.Now.AddMonths(-3));
            dv.Table.Columns[4].ColumnName = GetMonthName(DateTime.Now.AddMonths(-4));
            dv.Table.Columns[5].ColumnName = GetMonthName(DateTime.Now.AddMonths(-5));
            dv.Table.Columns[6].ColumnName = GetMonthName(DateTime.Now.AddMonths(-6));
            GridView = dv;
        }

        private void Query2()
        {
            var dv = new DataView(SQLDatabase.ExecuteQuery(Properties.Resources.Query2).Tables[0]);
            GridView = dv;
        }

        private void Query3()
        {
            var dv = new DataView(SQLDatabase.ExecuteQuery(Properties.Resources.Query3).Tables[0]);
            GridView = dv;
        }

        private string GetMonthName(DateTime date)
        {
            switch (date.Month)
            {
                case 1: return "Январь";
                case 2: return "Февраль";
                case 3: return "Март";
                case 4: return "Апрель";
                case 5: return "Май";
                case 6: return "Июнь";
                case 7: return "Июль";
                case 8: return "Август";
                case 9: return "Сентябрь";
                case 10: return "Октябрь";
                case 11: return "Ноябрь";
                case 12: return "Декабрь";
                default: return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace EmployeeRegistry.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public decimal BaseSalary { get; set; }
        public int? ChiefId { get; set; }
        public Employee Chief { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }

        public List<Employee> Subordinates { get; set; }

        //Расчет общей зарплаты с учетом подчиненых
        public decimal GetSalary(DateTime requestedDate) 
        {
            decimal salary = GetBaseSalary(requestedDate);
            foreach (var sub in Subordinates)
            {
                switch (Position.Id)
                {
                    case 2:
                        salary += GetBaseSalary(requestedDate);
                        break;
                    case 3:
                        salary += GetSalary(requestedDate);
                        break;
                    default:
                        break;
                }
            }
            return salary;
        }

        //Расчет базовой ставки + годовые проценты
        public decimal GetBaseSalary(DateTime requestedDate)
        {
            var experience = requestedDate.Year - EnrollmentDate.Year;
            if (EnrollmentDate.Date > requestedDate.Date.AddYears(-experience)) experience--;
            decimal salary;
            if (experience * Position.YearPercent >= Position.MaxYearPercent)
            {
                salary = BaseSalary + BaseSalary * experience * Position.MaxYearPercent;
            }
            else
            {
                salary = BaseSalary + BaseSalary * experience * Position.YearPercent;
            }
            return salary;
        }
    }
}

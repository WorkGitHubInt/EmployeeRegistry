using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using EmployeeRegistry.Models;
using System.Data;
using System.Linq;

namespace EmployeeRegistry
{
    public class SQLDatabase
    {
        public static List<Employee> Employees = new List<Employee>();
        public static List<Position> Positions = new List<Position>();

        public static void LoadData()
        {
            Employees.Clear();
            Positions.Clear();
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Employees";
                var command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            EnrollmentDate = DateTime.Parse(reader.GetString(2)),
                            BaseSalary = reader.GetDecimal(3),
                            PositionId = reader.GetInt32(5)
                        };
                        if (reader[4].GetType() != typeof(DBNull))
                        {
                            employee.ChiefId = reader.GetInt32(4);
                        }
                        Employees.Add(employee);
                    }
                }
                reader.Close();
                query = "SELECT * FROM Position";
                command = new SQLiteCommand(query, connection);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var position = new Position
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            YearPercent = reader.GetDecimal(2),
                            MaxYearPercent = reader.GetDecimal(3)
                        };
                        Positions.Add(position);
                    }
                }
                reader.Close();
            }
            foreach (var employee in Employees)
            {
                employee.Position = Positions.Find(p => p.Id == employee.PositionId);
                employee.Subordinates = Employees.Where(s => s.ChiefId == employee.Id).ToList();
            }
        }

        public static Employee LoginQuery(string login)
        {
            Employee employee = default;
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                string query = $"SELECT * FROM Employees WHERE Employees.Login = '{login}'";
                var command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            EnrollmentDate = DateTime.Parse(reader.GetString(2)),
                            BaseSalary = reader.GetDecimal(3),
                            PositionId = reader.GetInt32(5),
                            Login = reader.GetString(6),
                            Password = reader.GetString(7)
                        };
                        if (reader[4].GetType() != typeof(DBNull))
                        {
                            employee.ChiefId = reader.GetInt32(4);
                        }
                    }
                }
                reader.Close();
            }
            employee.Subordinates = Employees.Where(s => s.ChiefId == employee.Id).ToList();
            employee.Position = Positions.Find(p => p.Id == employee.PositionId);
            return employee;
        }

        public static DataSet ExecuteQuery(string query)
        {
            DataSet dataSet = new DataSet();
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, connection);
                dataAdapter.Fill(dataSet);
            }
            return dataSet;
        }

        public static void AddEmployee(Employee employee)
        {
            object chiefId;
            if (employee.Chief == null)
            {
                chiefId = null;
            } else
            {
                chiefId = employee.Chief.Id;
            }
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Employees VALUES (null, @Name, @EnrollmentDate, @BaseSalary, @ChiefId, @PositionId, @Login, @Password)", connection);
                command.Parameters.Add(new SQLiteParameter("@Name", employee.Name));
                command.Parameters.Add(new SQLiteParameter("@EnrollmentDate", employee.EnrollmentDate));
                command.Parameters.Add(new SQLiteParameter("@BaseSalary", employee.BaseSalary));
                command.Parameters.Add(new SQLiteParameter("@ChiefId", chiefId));
                command.Parameters.Add(new SQLiteParameter("@PositionId", employee.Position.Id));
                command.Parameters.Add(new SQLiteParameter("@Login", employee.Login));
                command.Parameters.Add(new SQLiteParameter("@Password", employee.Password));
                command.ExecuteNonQuery();
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static decimal RequestSalary(DateTime requestDate, int employeeId)
        {
            decimal salary = default;
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                string query = Properties.Resources.QuerySalary.Replace("date('now')", $"date('{requestDate.ToString("yyyy-MM-dd")}')").Replace("ID_TO_REPLACE", employeeId.ToString());
                var command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        salary = reader.GetDecimal(0);
                    }
                }
                reader.Close();
            }
            return salary;
        }

        public static decimal RequestAllSalary()
        {
            decimal salary = default;
            using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();
                string query = Properties.Resources.QueryAllSalary;
                var command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        salary = reader.GetDecimal(0);
                    }
                }
                reader.Close();
            }
            return salary;
        }

        private static string LoadConnectionString(string id = "SqlDBConnection")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}

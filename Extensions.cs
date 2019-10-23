using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EmployeeRegistry
{
    public static class DateTimeExtension
    {
        public static int YearDifference(this DateTime endDate, DateTime startDate)
        {
            return (int)Math.Floor((endDate - startDate).TotalDays / 365.250);
        }
    }

    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }
    }
}

using System;
using System.Collections.Generic;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    public abstract class Emploee
    {
        protected Dictionary<DateTime, int> hoursWorked { get; set; }

        public string Name { get; set; }
        
        public Emploee()
        {
            hoursWorked = new Dictionary<DateTime, int>();
        }

        public virtual void AddHoursWorked(DateTime date, int hours)
        {
            if (hours > 24 || hours < 0)
                throw new ArgumentException(nameof(hours));

            if (date > DateTime.Now || hoursWorked.ContainsKey(date.Date))
                throw new ArgumentException(nameof(date));

            hoursWorked.Add(date.Date, hours);
        }

        public abstract decimal CalculateSalary(DateTime from, DateTime to);

        public abstract Dictionary<DateTime, decimal> CalculateSalaryByDay(DateTime from, DateTime to);
    }
}

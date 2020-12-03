using System;
using System.Collections.Generic;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    public class Freelancer : Emploee
    {
        public const decimal SalaryPerHour = 1000;

        public Freelancer() : base() { }

        public Freelancer(string name, Dictionary<DateTime, HoursWorkedRecord> records) : base(name, records) { }


        public override void AddRecord(DateTime date, HoursWorkedRecord record)
        {
            if (date.Date > DateTime.Now.Date || date < DateTime.Now.AddDays(-2).Date
                || HasRecord(date))
            {
                throw new ArgumentException(nameof(date));
            }

            records.Add(date.Date, record);
        }

        public override decimal CalculateSalary(DateTime date)
        {
            if (records.ContainsKey(date.Date) == false)
                return 0;

            var dateRecord = records[date.Date];
            int workedHoursFromMonthStartToDate = GetTotalWorkedHours(date.Year, date.Month);
            int overtimeWorkedHours = workedHoursFromMonthStartToDate - MinimumMonthlyHours;
            decimal salary;

            if (overtimeWorkedHours > 0)
                salary = (dateRecord.Hours - overtimeWorkedHours) * SalaryPerHour;
            else
                salary = dateRecord.Hours * SalaryPerHour;

            return salary;
        }

        public override decimal CalculateSalary(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}

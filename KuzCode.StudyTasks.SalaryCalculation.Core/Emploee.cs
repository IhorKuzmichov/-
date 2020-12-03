using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    public abstract class Emploee
    {
        private string name;
        protected Dictionary<DateTime, HoursWorkedRecord> records;

        public const int MinimumMonthlyHours = 160;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Contains(","))
                    throw new ArgumentNullException();

                name = value;
            }
        }


        public Emploee()
        {
            records = new Dictionary<DateTime, HoursWorkedRecord>();
        }

        public Emploee(string name, Dictionary<DateTime, HoursWorkedRecord> records)
        {
            if (records is null)
                throw new ArgumentException(nameof(records));

            Name = name;
            this.records = records;
        }


        public bool HasRecord(DateTime date)
        {
            return records.ContainsKey(date.Date);
        }

        public HoursWorkedRecord GetRecord(DateTime date)
        {
            if (HasRecord(date))
                throw new ArgumentException(nameof(date));

            return records[date.Date];
        }

        public virtual void AddRecord(DateTime date, HoursWorkedRecord record)
        {
            if (date.Date > DateTime.Now.Date || HasRecord(date))
                throw new ArgumentException(nameof(date));

            this.records.Add(date.Date, record);
        }

        public int GetTotalWorkedHours(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException();

            return records
                .Where(record => record.Key >= from && record.Key <= to)
                .Select(record => record.Value.Hours)
                .Sum();
        }

        public int GetTotalWorkedHours(int year, int month)
        {
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1).AddDays(-1);

            return GetTotalWorkedHours(from, to);
        }

        public abstract decimal CalculateSalary(DateTime date);

        public abstract decimal CalculateSalary(DateTime from, DateTime to);


        /*protected abstract Emploee[] LoadEmploees();

        public abstract void Save();*/
    }
}

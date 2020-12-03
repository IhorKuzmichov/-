using System;
using System.Collections.Generic;
using System.Text;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    public class HoursWorkedRecord
    {
        private int hours;
        private string description;

        public int Hours
        {
            get
            {
                return hours;
            }

            set
            {
                if (value < 0 || value > 12)
                    throw new ArgumentNullException();

                hours = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Contains(","))
                    throw new ArgumentNullException();

                description = value;
            }
        }

        public HoursWorkedRecord(int hours, string description)
        {
            Hours = hours;
            Description = description;
        }
    }
}

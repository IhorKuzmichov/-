using System;
using System.Collections.Generic;
using System.Text;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    /// <summary>
    /// Запись сотрудника про отработанное время за какой-то день
    /// </summary>
    public class EmploeeRecord
    {
        private int hoursWorked;
        private string description;

        /// <summary>
        /// Количество отработанных часов
        /// </summary>
        public int HoursWorked
        {
            get
            {
                return hoursWorked;
            }

            set
            {
                if (value < 0 || value > 12)
                    throw new ArgumentNullException();

                hoursWorked = value;
            }
        }

        /// <summary>
        /// Описание (что было сделано за проработанное время)
        /// </summary>
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

        /// <param name="hours">Количество отработанных часов</param>
        /// <param name="description">Описание (что было сделано за проработанное время)</param>
        public EmploeeRecord(int hours, string description)
        {
            HoursWorked = hours;
            Description = description;
        }
    }
}

using System;
using System.Collections.Generic;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    /// <summary>
    /// Удалённый сотрудник (фрилансер)
    /// </summary>
    public class Freelancer : Emploee
    {
        public const decimal SalaryPerHour = 1000;

        /// <param name="name">Имя</param>
        public Freelancer(string name) : base(name) { }
        
        /// <summary>
        /// Конструктор для создания сотрудника с предзаготовленными записями
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="records">Записи об отработанных часах работы. Ключ-дата должна быть без указания времени (0 часов, 0 минут, 0 секунд)</param>
        public Freelancer(string name, Dictionary<DateTime, EmploeeRecord> records) : base(name, records) { }


        /// <summary>
        /// Добавить запись
        /// <para>ArgumentException: вызывается если запись с такой <paramref name="date"/> уже присутствует, если <paramref name="date"/> позднее текущей даты или более ранняя чем за два дня от текущей даты</para>
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="record">Запись об отработанных часах работы</param>
        /// <exception cref="ArgumentException"></exception>
        public override void AddRecord(DateTime date, EmploeeRecord record)
        {
            if (date.Date > DateTime.Now.Date ||
                date.Date < DateTime.Now.AddDays(-2).Date ||
                records.ContainsKey(date.Date))
            {
                throw new ArgumentException("", nameof(date));
            }

            records.Add(date.Date, record);
        }

        /// <summary>
        /// Рассчитать зарплату
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Количество заработанных денег за <paramref name="date"/></returns>
        public override decimal CalculateSalary(DateTime date)
        {
            if (records.ContainsKey(date.Date) == false)
                return 0;

            var dateRecord = records[date.Date];
            int hoursWorkedFromMonthStartToDate = GetTotalHoursWorked(date.Year, date.Month);
            int overtimeWorkedHours = hoursWorkedFromMonthStartToDate - MinimumMonthlyHours;
            decimal salary;

            if (overtimeWorkedHours > 0)
                salary = (dateRecord.HoursWorked - overtimeWorkedHours) * SalaryPerHour;
            else
                salary = dateRecord.HoursWorked * SalaryPerHour;

            return salary;
        }

        /// <summary>
        /// Рассчитать зарплату
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns>Количество заработанных денег за месяц <paramref name="month"/> года <paramref name="year"/></returns>
        public override decimal CalculateSalary(int year, int month)
        {
            var monthlyHoursWorked = GetTotalHoursWorked(year, month);
            decimal salary;

            if (monthlyHoursWorked > MinimumMonthlyHours)
                salary = MinimumMonthlyHours * SalaryPerHour;
            else
                salary = monthlyHoursWorked * SalaryPerHour;

            return salary;
        }
    }
}

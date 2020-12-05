using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KuzCode.StudyTasks.SalaryCalculation.Core
{
    /// <summary>
    /// Информация о сотруднике
    /// </summary>
    public abstract class Emploee
    {
        private string name;
        protected Dictionary<DateTime, EmploeeRecord> records;

        /// <summary>
        /// Минимальное количество требуемых отработанных часов работы в месяц
        /// </summary>
        public const int MinimumMonthlyHours = 160;

        /// <summary>
        /// Имя сотрудника
        /// </summary>
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

        /// <summary>
        /// Записи об отработанных часах работы (только для чтения). Для получении записи по ключу-дате, дата должна быть без указания времени (0 часов, 0 минут, 0 секунд)
        /// </summary>
        public ReadOnlyDictionary<DateTime, EmploeeRecord> ReadOnlyRecords
        {
            get
            {
                return records is null ? null : new ReadOnlyDictionary<DateTime, EmploeeRecord>(records);
            }
        }


        /// <param name="name">Имя</param>
        public Emploee(string name)
        {
            Name = name;
            records = new Dictionary<DateTime, EmploeeRecord>();
        }

        /// <param name="name">Имя сотрудника</param>
        /// <param name="records">Записи об отработанных часах работы. Ключ-дата должна быть без указания времени (0 часов, 0 минут, 0 секунд)</param>
        public Emploee(string name, Dictionary<DateTime, EmploeeRecord> records)
        {
            if (records is null || records.Any(record => record.Key != record.Key.Date))
                throw new ArgumentException(nameof(records));

            Name = name;
            this.records = records;
        }


        /// <summary>
        /// Добавить запись
        /// <para>ArgumentException: вызывается если запись с такой <paramref name="date"/> уже присутствует или если <paramref name="date"/> позднее текущей даты</para>
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="record">Запись об отработанных часах работы</param>
        /// <exception cref="ArgumentException"></exception>
        public virtual void AddRecord(DateTime date, EmploeeRecord record)
        {
            if (date.Date > DateTime.Now.Date || records.ContainsKey(date.Date))
                throw new ArgumentException("", nameof(date));

            records.Add(date.Date, record);
        }

        /// <summary>
        /// Получить записи об отработанных часах работы
        /// </summary>
        /// <param name="from">От (дата)</param>
        /// <param name="to">До (дата)</param>
        /// <returns>Записи об отработанных часах работы за период с <paramref name="from"/> по <paramref name="to"/></returns>
        public ReadOnlyDictionary<DateTime, EmploeeRecord> GetRecords(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException();

            var foundRecords = records
                .Where(record => record.Key >= from && record.Key <= to)
                .ToDictionary(record => record.Key, record => record.Value);

            return new ReadOnlyDictionary<DateTime, EmploeeRecord>(foundRecords);
        }

        /// <summary>
        /// Получить записи об отработанных часах работы
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns>Количество заработанных денег за месяц <paramref name="month"/> года <paramref name="year"/></returns>
        public ReadOnlyDictionary<DateTime, EmploeeRecord> GetRecords(int year, int month)
        {
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1).AddDays(-1);

            return GetRecords(from, to);
        }

        /// <summary>
        /// Получить количество отработанных часов
        /// </summary>
        /// <param name="from">От (дата)</param>
        /// <param name="to">До (дата)</param>
        /// <returns>Количество отработанных часов за пеиод от <paramref name="from"/> по <paramref name="to"/></returns>
        public int GetTotalHoursWorked(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException();

            return records
                .Where(record => record.Key >= from && record.Key <= to)
                .Select(record => record.Value.HoursWorked)
                .Sum();
        }

        /// <summary>
        /// Получить количество отработанных часов
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <returns>Количество отработанных часов за месяц <paramref name="month"/> года <paramref name="year"/></returns>
        public int GetTotalHoursWorked(int year, int month)
        {
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1).AddDays(-1);

            return GetTotalHoursWorked(from, to);
        }

        public abstract decimal CalculateSalary(DateTime date);

        public abstract decimal CalculateSalary(int year, int month);


        /*protected abstract Emploee[] LoadEmploees();

        public abstract void Save();*/
    }
}

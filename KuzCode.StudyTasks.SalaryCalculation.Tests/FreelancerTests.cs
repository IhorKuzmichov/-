using KuzCode.StudyTasks.SalaryCalculation.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KuzCode.StudyTasks.SalaryCalculation.Core.Tests
{
    [TestClass]
    public class FreelancerTests
    {
        [TestMethod]
        public void InitializeFreelancerWithRecords_RecordsWithDateAndTime_ArgumentExceptionIsThrown()
        {
            var records = new Dictionary<DateTime, EmploeeRecord>();
            records.Add(new DateTime(2020, 1, 1, 10, 10, 10), new EmploeeRecord(8, "Работал"));

            try
            {
                var freelancer = new Freelancer("Работник", records);
            }
            catch (ArgumentException)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void CalculateSalary_AddTodayHoursWorked_NoExceptionIsThrown()
        {
            var freelancer = new Freelancer("Работник");
            var currentDate = DateTime.Now;
            var hoursWorked = new EmploeeRecord(8, "Работал");

            try
            {
                freelancer.AddRecord(currentDate, hoursWorked);
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [TestMethod]
        public void CalculateSalary_Add5DaysAgoHoursWorked_ArgumentExceptionIsThrown()
        {
            var freelancer = new Freelancer("Работник");
            var currentDate = DateTime.Now.AddDays(-5);
            var hoursWorked = new EmploeeRecord(8, "Работал");

            try
            {
                freelancer.AddRecord(currentDate, hoursWorked);
            }
            catch (ArgumentException)
            {
                return;
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }

            Assert.Fail();
        }

        [TestMethod]
        public void GetTotalWorkedHours_8And9And3_20Returned()
        {
            var records = new Dictionary<DateTime, EmploeeRecord>();
            records.Add(new DateTime(2020, 1, 1), new EmploeeRecord(8, "Работал"));
            records.Add(new DateTime(2020, 1, 2), new EmploeeRecord(9, "Работал"));
            records.Add(new DateTime(2020, 1, 3), new EmploeeRecord(3, "Работал"));

            var freelancer = new Freelancer("Имя", records);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 1, 3);

            if (freelancer.GetTotalHoursWorked(from, to) != 20)
                Assert.Fail();
        }

        [TestMethod]
        public void CalculateSalaryForThirdDay_23HoursFromStartMonthToThirdDay_6000Returned()
        {
            var records = new Dictionary<DateTime, EmploeeRecord>();
            records.Add(new DateTime(2020, 1, 1), new EmploeeRecord(8, "Работал"));
            records.Add(new DateTime(2020, 1, 2), new EmploeeRecord(7, "Работал"));
            records.Add(new DateTime(2020, 1, 3), new EmploeeRecord(8, "Работал"));

            var freelancer = new Freelancer("Имя", records);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 1, 3);

            if (freelancer.GetTotalHoursWorked(from, to) != 23) // в сумме должно быть 23 часа (проверка самого себя)
                Assert.Fail();

            var date = new DateTime(2020, 1, 3);
            decimal expected = 8000; // 8 часов - 0 часов переработки * 1000руб/час
            var actual = freelancer.CalculateSalary(date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateSalaryFor23thDayWith8Hours_165HoursFromStartMonthTo23thDay_6000Returned()
        {
            var records = new Dictionary<DateTime, EmploeeRecord>();

            for (int i = 1; i <= 22; i++) // 154 часа
                records.Add(new DateTime(2020, 1, i), new EmploeeRecord(7, "Работал"));

            records.Add(new DateTime(2020, 1, 23), new EmploeeRecord(8, "Работал")); // 162 часов - переработка 2 часа

            var freelancer = new Freelancer("Имя", records);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 1, 23);

            if (freelancer.GetTotalHoursWorked(from, to) != 162) // в сумме должно быть 162 часа в месяц (проверка самого себя)
                Assert.Fail();

            var date = new DateTime(2020, 1, 23);
            decimal expected = 6000; // 8 часов - 2 часа переработки * 1000руб/час
            var actual = freelancer.CalculateSalary(date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateSalaryForNovember2020_165HoursWorked_160000Returned()
        {
            var records = new Dictionary<DateTime, EmploeeRecord>();

            for (int i = 1; i <= 22; i++) // 154 часа
                records.Add(new DateTime(2020, 1, i), new EmploeeRecord(7, "Работал"));

            records.Add(new DateTime(2020, 1, 23), new EmploeeRecord(8, "Работал")); // 162 часов - переработка 2 часа

            var freelancer = new Freelancer("Имя", records);

            if (freelancer.GetTotalHoursWorked(2020, 1) != 162) // в сумме должно быть 162 часа в месяц (проверка самого себя)
                Assert.Fail();

            decimal expected = 160000; // 160 * 1000руб/час (переработки не в счёт)
            var actual = freelancer.CalculateSalary(2020, 1);

            Assert.AreEqual(expected, actual);
        }
    }
}
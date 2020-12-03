using Microsoft.VisualStudio.TestTools.UnitTesting;
using KuzCode.StudyTasks.SalaryCalculation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuzCode.StudyTasks.SalaryCalculation.Core.Tests
{
    [TestClass()]
    public class FreelancerTests
    {
        [TestMethod()]
        public void CalculateSalary_AddTodayHoursWorked_NoExceptionIsThrown()
        {
            var freelancer = new Freelancer();
            var currentDate = DateTime.Now;
            var hoursWorked = new HoursWorkedRecord(8, "Работал");

            try
            {
                freelancer.AddRecord(currentDate, hoursWorked);
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [TestMethod()]
        public void CalculateSalary_Add5DaysAgoHoursWorked_ExceptionIsThrown()
        {
            var freelancer = new Freelancer();
            var currentDate = DateTime.Now.AddDays(-5);
            var hoursWorked = new HoursWorkedRecord(8, "Работал");

            try
            {
                freelancer.AddRecord(currentDate, hoursWorked);
            }
            catch (ArgumentException exception)
            {
                return;
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void GetTotalWorkedHours_8And9And3_20Returned()
        {
            var records = new Dictionary<DateTime, HoursWorkedRecord>();
            records.Add(new DateTime(2020, 1, 1), new HoursWorkedRecord(8, "Работал"));
            records.Add(new DateTime(2020, 1, 2), new HoursWorkedRecord(9, "Работал"));
            records.Add(new DateTime(2020, 1, 3), new HoursWorkedRecord(3, "Работал"));

            var freelancer = new Freelancer("Имя", records);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 1, 3);

            if (freelancer.GetTotalWorkedHours(from, to) != 20)
                Assert.Fail();
        }

        [TestMethod()]
        public void CalculateSalaryForThirdDay_23HoursFromStartMonthToThirdDay_6000Returned()
        {
            var records = new Dictionary<DateTime, HoursWorkedRecord>();
            records.Add(new DateTime(2020, 1, 1), new HoursWorkedRecord(8, "Работал"));
            records.Add(new DateTime(2020, 1, 2), new HoursWorkedRecord(7, "Работал"));
            records.Add(new DateTime(2020, 1, 3), new HoursWorkedRecord(8, "Работал"));

            var freelancer = new Freelancer("Имя", records);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 1, 3);

            if (freelancer.GetTotalWorkedHours(from, to) != 23) // в сумме должно быть 23 часа (проверка самого себя)
                Assert.Fail();

            var date = new DateTime(2020, 1, 3);
            decimal expected = 8 * Freelancer.SalaryPerHour; // 8 часов - 0 часов переработки * 1000руб/час
            var actual = freelancer.CalculateSalary(date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CalculateSalaryFor23thDayWith8Hours_165HoursFromStartMonthTo23thDay_6000Returned()
        {
            var records = new Dictionary<DateTime, HoursWorkedRecord>();

            for (int i = 1; i <= 22; i++) // 154 часа
                records.Add(new DateTime(2020, 1, i), new HoursWorkedRecord(7, "Работал"));

            records.Add(new DateTime(2020, 1, 23), new HoursWorkedRecord(8, "Работал")); // 162 часов - переработка 2 часа

            var freelancer = new Freelancer("Имя", records);
            var from = new DateTime(2020, 1, 1);
            var to = new DateTime(2020, 1, 23);

            if (freelancer.GetTotalWorkedHours(from, to) != 162) // в сумме должно быть 162 часа в месяц (проверка самого себя)
                Assert.Fail();

            var date = new DateTime(2020, 1, 23);
            decimal expected = 6 * Freelancer.SalaryPerHour; // 8 часов - 2 часа переработки * 1000руб/час
            var actual = freelancer.CalculateSalary(date);

            Assert.AreEqual(expected, actual);
        }
    }
}
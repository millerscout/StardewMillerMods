using System;
using EconomyMod.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EconomyModTest
{
    [TestClass]
    public class WorldDateTests
    {
        [TestMethod]
        public void CheckIfDaysSeasonAndYearAreParsedCorrectly()
        {
            var FirstDayOfFirstYearOFSpring = 1.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Monday, FirstDayOfFirstYearOFSpring.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Spring, FirstDayOfFirstYearOFSpring.Season);
            Assert.AreEqual(1, FirstDayOfFirstYearOFSpring.Year);

            var ThursdayOnFirstYearOfSpring = 18.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Thursday, ThursdayOnFirstYearOfSpring.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Spring, ThursdayOnFirstYearOfSpring.Season);
            Assert.AreEqual(1, ThursdayOnFirstYearOfSpring.Year);

            var LastDayOfweekOnSpringOfFirstYear = 7.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Sunday, LastDayOfweekOnSpringOfFirstYear.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Spring, LastDayOfweekOnSpringOfFirstYear.Season);
            Assert.AreEqual(1, LastDayOfweekOnSpringOfFirstYear.Year);

            var SecondWeekMondayFirstYearFirst = 8.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Monday, SecondWeekMondayFirstYearFirst.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Spring, SecondWeekMondayFirstYearFirst.Season);
            Assert.AreEqual(1, SecondWeekMondayFirstYearFirst.Year);


            var FirstDayOfSecondSummer = 29.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Monday, FirstDayOfSecondSummer.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Summer, FirstDayOfSecondSummer.Season);
            Assert.AreEqual(1, FirstDayOfSecondSummer.Year);

            var LastDayOfYear = 112.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Sunday, LastDayOfYear.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Winter, LastDayOfYear.Season);
            Assert.AreEqual(1, LastDayOfYear.Year);

            var FirstDayOfSecondYear = 113.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Monday, FirstDayOfSecondYear.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Spring, FirstDayOfSecondYear.Season);
            Assert.AreEqual(2, FirstDayOfSecondYear.Year);


            var SummerAtSecondYear = 168.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Sunday, SummerAtSecondYear.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Summer, SummerAtSecondYear.Season);
            Assert.AreEqual(2, SummerAtSecondYear.Year);


            var LastDayOfSecondYear = 224.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Sunday, LastDayOfSecondYear.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Winter, LastDayOfSecondYear.Season);
            Assert.AreEqual(2, LastDayOfSecondYear.Year);

            var FirstDayOfThirdYear = 225.ToWorldDate();
            Assert.AreEqual(DayOfWeek.Monday, FirstDayOfThirdYear.Day);
            Assert.AreEqual(EconomyMod.Model.Season.Spring, FirstDayOfThirdYear.Season);
            Assert.AreEqual(3, FirstDayOfThirdYear.Year);

        }
        [TestMethod]
        public void CheckIfNextDayIsValid()
        {

            var scenarioOne = 1.ToWorldDate().Next(DayOfWeek.Monday);
            Assert.IsTrue(scenarioOne.DaysCount > 1);
            Assert.AreEqual(DayOfWeek.Monday, scenarioOne.Day);

            var nextday = DayOfWeek.Tuesday;
            var scenarioTwo = 1.ToWorldDate();
            Assert.AreNotEqual(nextday, scenarioTwo.Day);
            scenarioTwo.Next(DayOfWeek.Tuesday);

            Assert.IsTrue(scenarioTwo.DaysCount > 1);
            Assert.AreEqual(DayOfWeek.Tuesday, scenarioTwo.Day);


        }
        [TestMethod]
        public void CheckIfDaysOfMonthIsRight()
        {
            CheckScenario(1, 1);
            CheckScenario(28, 28);
            CheckScenario(29, 1);
            CheckScenario(35, 7);
            CheckScenario(18, 18);
            CheckScenario(28, 28);
            CheckScenario(113, 1);
            CheckScenario(141, 1);
            CheckScenario(140, 28);
            CheckScenario(1579, 11); 

            EconomyMod.Model.CustomWorldDate CheckScenario(int day, int expected)
            {
                var scenarioOne = day.ToWorldDate();
                Assert.AreEqual(scenarioOne.DayOfMonth, expected);
                return scenarioOne;
            }
        }

    }
}

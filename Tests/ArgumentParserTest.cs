using System;
using System.Linq;
using MeetingRoomHelper;
using MeetingRoomHelper.Core;
using NUnit.Framework;

namespace MeetingRoomHelperTest
{
    [TestFixture]
    public class ArgumentParserTest
    {
        [Test]
        public void DateArgument_AdjustsTimeWhenNotSpecified()
        {
            var args = new []{"-d", "11/22/2016"};
            var parser = new ArgumentParser(args, TimeHelper.CreateTime(14, 22, 11));

            Assert.AreEqual(new DateTime(2016, 11, 22).Date, parser.DateToCheck.Date);
            Assert.AreEqual(14, parser.DateToCheck.Hour);
            Assert.AreEqual(15, parser.DateToCheck.Minute);
            Assert.AreEqual(00, parser.DateToCheck.Second);
            Assert.IsFalse(parser.PrintHelp);

            args = new[] { "/D", "11/22/2016" };
            parser = new ArgumentParser(args, TimeHelper.CreateTime(14, 22, 11));

            Assert.AreEqual(new DateTime(2016, 11, 22).Date, parser.DateToCheck.Date);
            Assert.AreEqual(14, parser.DateToCheck.Hour);
            Assert.AreEqual(15, parser.DateToCheck.Minute);
            Assert.AreEqual(00, parser.DateToCheck.Second);
        }

        [Test]
        public void TimeArgument()
        {
            var args = new[] { "-T", "15:00" };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(DateTime.Today.Date, parser.DateToCheck.Date);
            Assert.AreEqual(15, parser.DateToCheck.Hour);
            Assert.AreEqual(00, parser.DateToCheck.Minute);

            args = new[] { "/t", "15:00" };
            parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(DateTime.Today.Date, parser.DateToCheck.Date);
            Assert.AreEqual(15, parser.DateToCheck.Hour);
            Assert.AreEqual(00, parser.DateToCheck.Minute);
        }

        [Test]
        public void TimeArgument_GetAdjusted()
        {
            var args = new[] { "-t", "15:11" };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(DateTime.Today.Date, parser.DateToCheck.Date);
            Assert.AreEqual(15, parser.DateToCheck.Hour);
            Assert.AreEqual(00, parser.DateToCheck.Minute);
        }

        [Test]
        public void DateAndTimeArgument()
        {
            var args = new[] { "-d", "2/2/2016", "-t", "9:30" };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(new DateTime(2016, 2, 2).Date, parser.DateToCheck.Date);
            Assert.AreEqual(9, parser.DateToCheck.Hour);
            Assert.AreEqual(30, parser.DateToCheck.Minute);
        }

        [Test]
        public void TimeArgument_Morning()
        {
            var args = new[] { "-t", "morning" };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(DateTime.MinValue, parser.DateToCheck);
            Assert.AreEqual((60 / TimeHelper.TimeInterval) * 4, parser.DateRangeToCheck.Count);
            Assert.AreEqual(8, parser.DateRangeToCheck.First().Hour);
            Assert.AreEqual(00, parser.DateRangeToCheck.First().Minute);
            Assert.AreEqual(11, parser.DateRangeToCheck.Last().Hour);
            Assert.AreEqual(45, parser.DateRangeToCheck.Last().Minute);
        }

        [Test]
        public void TimeArgument_Afternoon()
        {
            var args = new[] { "-t", "afternoon" };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(DateTime.MinValue, parser.DateToCheck);
            Assert.AreEqual((60 / TimeHelper.TimeInterval) * 5, parser.DateRangeToCheck.Count);
            Assert.AreEqual(12, parser.DateRangeToCheck.First().Hour);
            Assert.AreEqual(00, parser.DateRangeToCheck.First().Minute);
            Assert.AreEqual(16, parser.DateRangeToCheck.Last().Hour);
            Assert.AreEqual(45, parser.DateRangeToCheck.Last().Minute);
        }

        [Test]
        public void TimeArgument_AllDay()
        {
            var args = new[] { "-t", "allday" };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.AreEqual(DateTime.MinValue, parser.DateToCheck);
            Assert.AreEqual((60 / TimeHelper.TimeInterval) * 9, parser.DateRangeToCheck.Count);
            Assert.AreEqual(8, parser.DateRangeToCheck.First().Hour);
            Assert.AreEqual(00, parser.DateRangeToCheck.First().Minute);
            Assert.AreEqual(16, parser.DateRangeToCheck.Last().Hour);
            Assert.AreEqual(45, parser.DateRangeToCheck.Last().Minute);
        }

        [TestCase("?")]
        [TestCase("help")]
        [TestCase("man")]
        public void Help(string arg)
        {
            var args = new[] { arg };
            var parser = new ArgumentParser(args, DateTime.Now);

            Assert.IsTrue(parser.PrintHelp);
        }
    }
}

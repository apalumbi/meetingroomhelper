using System;
using MeetingRoomHelper.Core;
using MeetingRoomHelper.Core.DomainOjects;
using NUnit.Framework;

namespace MeetingRoomHelperTest
{
    [TestFixture]
    public class MeetingRoomTest
    {
        [Test]
        public void SetsUpOpenTimesProperly()
        {
            var meetingRoom = new MeetingRoom("foo", TestDataFactory.GetWeekend);
            Assert.AreEqual(TimeHelper.SegmentsInDay * 2, meetingRoom.OpenTimes.Count);
        }

        [Test]
        public void FreeAtTime()
        {
            var meetingRoom = new MeetingRoom("foo", TestDataFactory.GetOneDayLunchFree);
            var lunchTime = TimeHelper.CreateTime(12, 00, 00);

            Assert.IsTrue(meetingRoom.IsFreeAt(lunchTime));

            var breakfastTime = TimeHelper.CreateTime(08, 00, 00);
            Assert.IsFalse(meetingRoom.IsFreeAt(breakfastTime));

            var dinnerTime = TimeHelper.CreateTime(18, 00, 00);
            Assert.IsFalse(meetingRoom.IsFreeAt(dinnerTime));
        }

        [Test]
        public void ValidateInput()
        {
            Assert.Throws<ArgumentException>(() => new MeetingRoom("bar", TestDataFactory.BadFreeBusyShort));
            Assert.Throws<ArgumentException>(() => new MeetingRoom("foo", TestDataFactory.BadFreeBusyLong));
            Assert.Throws<ArgumentException>(() => new MeetingRoom("", TestDataFactory.GetFullFreeBusy));
        }

        [TestCase(1)]
        [TestCase(11)]
        [TestCase(27)]
        [TestCase(55)]
        public void IsFreeAt_Validates_Input_Minutes(int minute)
        {
            var meetingRoom = new MeetingRoom("foo", TestDataFactory.GetOneDayLunchFree);
            var timeToCheck = TimeHelper.CreateTime(12, minute, 00);

            Assert.Throws<ArgumentException>(() => meetingRoom.IsFreeAt(timeToCheck));
        }

        [TestCase(1)]
        [TestCase(11)]
        [TestCase(27)]
        [TestCase(55)]
        public void IsFreeAt_Validates_Input_Seconds(int second)
        {
            var meetingRoom = new MeetingRoom("foo", TestDataFactory.GetOneDayLunchFree);
            var timeToCheck = TimeHelper.CreateTime(12, 00, second);

            Assert.Throws<ArgumentException>(() => meetingRoom.IsFreeAt(timeToCheck));
        }

        [TestCase(00)]
        [TestCase(15)]
        [TestCase(30)]
        [TestCase(45)]
        public void FreeAtTime_Checks_All_Time(int minute)
        {
            var meetingRoom = new MeetingRoom("foo", TestDataFactory.GetOneDayLunchFree);
            var timeToCheck = TimeHelper.CreateTime(12, minute, 00);

            Assert.IsTrue(meetingRoom.IsFreeAt(timeToCheck));
        }

        [Test]
        public void FreeAtTime_Ignores_OtherDays()
        {
            var meetingRoom = new MeetingRoom("foo", TestDataFactory.GetTomorrowDayLunchFree);
            var timeToCheckTomorrow = TimeHelper.CreateTime(12, 00, 00).AddDays(1);
            Assert.IsTrue(meetingRoom.IsFreeAt(timeToCheckTomorrow));

            var timeToCheckToday = TimeHelper.CreateTime(12, 00, 00);
            Assert.IsFalse(meetingRoom.IsFreeAt(timeToCheckToday));

            var someFutureDate = TimeHelper.CreateTime(12, 00, 00).AddDays(100);
            Assert.IsFalse(meetingRoom.IsFreeAt(someFutureDate));
        }
    }
}

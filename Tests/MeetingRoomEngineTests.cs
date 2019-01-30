using System.Collections.Generic;
using MeetingRoomHelper.Core;
using MeetingRoomHelper.Core.DomainOjects;
using MeetingRoomHelper.Engine;
using NUnit.Framework;

namespace MeetingRoomHelperTest
{
    [TestFixture]
    public class MeetingRoomEngineTests
    {
        [Test]
        public void GetFreeRooms()
        {
            var engine = new MeetingRoomEngine();

            var room = new MeetingRoom("foo", TestDataFactory.GetOneDayLunchFree);
            var freeTimeToCheck = TimeHelper.CreateTime(12, 15, 00);
            Assert.IsNotEmpty(engine.GetFreeRooms(new List<MeetingRoom> { room }, freeTimeToCheck));

            var busyTime1 = TimeHelper.CreateTime(11, 15, 00);
            Assert.IsEmpty(engine.GetFreeRooms(new List<MeetingRoom> { room }, busyTime1));

            var busyTime2 = TimeHelper.CreateTime(13, 15, 00);
            Assert.IsEmpty(engine.GetFreeRooms(new List<MeetingRoom> { room }, busyTime2));
        }

        [Test]
        public void GetFreeTimeAsOf()
        {
            var engine = new MeetingRoomEngine();

            var room = new MeetingRoom("foo", TestDataFactory.GetOneDayLunchFree);
            var freeTimeToCheck = TimeHelper.CreateTime(12, 15, 00);
            Assert.AreEqual(45, engine.GetFreeTimeAsOf(room, freeTimeToCheck));
        }

        [Test]
        public void GetFreeTimeAsOf_DoesntGoPastSixPM()
        {
            var engine = new MeetingRoomEngine();

            var room = new MeetingRoom("foo", TestDataFactory.GetOneDayAllFree);
            var freeTimeToCheck = TimeHelper.CreateTime(16, 30, 00);
            Assert.AreEqual(90, engine.GetFreeTimeAsOf(room, freeTimeToCheck));
        }
    }
}

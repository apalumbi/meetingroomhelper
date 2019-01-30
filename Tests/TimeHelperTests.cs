using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingRoomHelper.Core;
using NUnit.Framework;

namespace MeetingRoomHelperTest
{
    [TestFixture]
    public class TimeHelperTests
    {
        [Test]
        public void AdjustTimeToNearestInterval()
        {
            Assert.AreEqual(15, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 16, 00)).Minute);
            Assert.AreEqual(45, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 59, 00)).Minute);
            Assert.AreEqual(30, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 39, 00)).Minute);
            Assert.AreEqual(00, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 06, 00)).Minute);
            Assert.AreEqual(0, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 16, 1)).Second);
            Assert.AreEqual(0, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 16, 26)).Second);
            Assert.AreEqual(0, TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(10, 16, 59)).Second);

        }
    }
}

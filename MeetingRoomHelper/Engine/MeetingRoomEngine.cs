using System;
using System.Collections.Generic;
using System.Linq;
using MeetingRoomHelper.Core;
using MeetingRoomHelper.Core.DomainOjects;
using MeetingRoomHelper.Core.Interfaces;

namespace MeetingRoomHelper.Engine
{
    public class MeetingRoomEngine : IMeetingRoomEngine
    {
        public List<MeetingRoom> GetFreeRooms(List<MeetingRoom> rooms, DateTime timeToCheck)
        {
            return rooms.Where(r => r.IsFreeAt(timeToCheck)).ToList();
        }

        public int GetFreeTimeAsOf(MeetingRoom room, DateTime timeToCheck)
        {
            var freeTimeDuration = 0;
            while (room.IsFreeAt(timeToCheck) && timeToCheck.Hour < TimeHelper.MaxHour)
            {
                freeTimeDuration += TimeHelper.TimeInterval;
                timeToCheck = timeToCheck.AddMinutes(TimeHelper.TimeInterval);
            }
            return freeTimeDuration;
        }
    }
}

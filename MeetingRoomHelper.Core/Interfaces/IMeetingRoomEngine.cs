using System;
using System.Collections.Generic;
using MeetingRoomHelper.Core.DomainOjects;

namespace MeetingRoomHelper.Core.Interfaces
{
    public interface IMeetingRoomEngine
    {
        List<MeetingRoom> GetFreeRooms(List<MeetingRoom> rooms, DateTime timeToCheck);
        int GetFreeTimeAsOf(MeetingRoom rooms, DateTime timeToCheck);
    }
}

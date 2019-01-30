using System;
using System.Collections.Generic;
using MeetingRoomHelper.Core.DomainOjects;

namespace MeetingRoomHelper.Core.Interfaces
{
    public interface IConferenceRoomAccess
    {
        List<MeetingRoom> GetMeetingRooms();
        void ScheduleMeeting(string roomName, int duration, DateTime startTime);
    }
}


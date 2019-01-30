using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MeetingRoomHelper.Core;
using MeetingRoomHelper.Core.DomainOjects;
using MeetingRoomHelper.Core.Interfaces;
using NetOffice.OutlookApi;
using NetOffice.OutlookApi.Enums;

namespace MeetingRoomHelper.ResourceAccess
{

    public class ConferenceRoomAccess: IConferenceRoomAccess
    {
        public List<MeetingRoom> GetMeetingRooms()
        {


            var session = Application.GetActiveInstance(true).Session;
            var addressLists = session.AddressLists.Where(al => al.Name == "All Rooms").ToList();

            var rooms = new List<MeetingRoom>();
            foreach (var list in addressLists)
            {
                foreach (var address in list.AddressEntries)
                {
                    rooms.Add(new MeetingRoom(address.Name, address.GetFreeBusy(DateTime.Today, TimeHelper.TimeInterval)));
                }
            }

            var confRooms = ConfigurationManager.AppSettings["MeetingRoomFilter"];
            if (String.IsNullOrEmpty(confRooms))
            {
                return rooms;
            }

            return rooms.Where(r => confRooms.Split(';').Contains(r.Name)).ToList();
        }

        public void ScheduleMeeting(string roomName, int duration, DateTime startTime)
        {
            var outlook = Application.GetActiveInstance();
            var appointment = outlook.CreateItem(OlItemType.olAppointmentItem) as AppointmentItem;
            appointment.Subject = "placeholder";
            appointment.MeetingStatus = OlMeetingStatus.olMeeting;
            appointment.Location = roomName;
            appointment.BusyStatus = OlBusyStatus.olBusy;
            appointment.IsOnlineMeeting = true;
            appointment.Start = startTime;
            appointment.Duration = duration;

            var sentTo = appointment.Recipients;
            Recipient sentInvite = null;
            sentInvite = sentTo.Add(roomName);
            sentInvite.Type = (int)OlMeetingRecipientType.olResource;
            sentTo.ResolveAll();

            appointment.Send();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MeetingRoomHelper.Core.DomainOjects
{
    public class MeetingRoom
    {
        public string Name { get; }

        public List<DateTime> OpenTimes { get; }

        public MeetingRoom(string name, string freeBusy)
        {
            if (string.IsNullOrEmpty(name) || freeBusy.Length % TimeHelper.SegmentsInDay != 0)
            {
                throw new ArgumentException("Malformed FreeBusy Input");
            }

            Name = name;
            OpenTimes = new List<DateTime>();
            var dayindex = 0;
            for (int i = 0; i < freeBusy.Length; i = i + TimeHelper.SegmentsInDay)
            {
                var time = TimeHelper.CreateTime(00, 00, 00).AddDays(dayindex);
                foreach (var fb in freeBusy.Skip(i).Take(TimeHelper.SegmentsInDay))
                {
                    if (fb == '0')
                    {
                        OpenTimes.Add(time);
                    }

                    time = time.AddMinutes(TimeHelper.TimeInterval);
                }
                dayindex++;
            }
        }

        public bool IsFreeAt(DateTime timeToCheck)
        {
            if (timeToCheck.Minute % TimeHelper.TimeInterval != 0 || timeToCheck.Second != 0)
            {
                throw new ArgumentException("Time must be sanitezed before Checking.");
            }
            
            return OpenTimes.Any(o => o == timeToCheck);           
        }
    }
}

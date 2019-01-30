using System;
using System.Text;

namespace MeetingRoomHelper.Core
{
    public static class Printer
    {
        public static string HelpText
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("Helps to book meeting rooms");
                sb.AppendLine("");
                sb.AppendLine("USAGE :");
                sb.AppendLine("MEETINGROOMHELPER [/T hh:mm] [/D mo/dd/yyyy]");
                sb.AppendLine("/T    The Time of the potential meeting. Format 16:30.  Defaults to now");
                sb.AppendLine("/D    The Date of the potential meeting. Format 12/01/2016.  Defaults to today");

                return sb.ToString();
            }
        }
        public static string PrintStatus(string roomName, DateTime dateToCheck, int freeTime)
        {
            return $"{roomName} is free {dateToCheck.ToString("HH:mm")} - {dateToCheck.AddMinutes(freeTime).ToString("HH:mm")} ({freeTime} mins)";
        }

    }
}

using System;
using System.Linq;
using System.Windows.Forms;
using MeetingRoomHelper.Core;
using MeetingRoomHelper.Engine;
using MeetingRoomHelper.ResourceAccess;

namespace MeetingRoomHelper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var arguments = new ArgumentParser(args, DateTime.Now);
                if (arguments.PrintHelp)
                {
                    PromptAndWait(Printer.HelpText);
                }
                else
                {
                    ExectDatePath(arguments.DateToCheck);
                }
            }
            catch (Exception ex)
            {
                PromptAndWait(ex.Message);
            }
        }

        private static void ExectDatePath(DateTime dateToCheck)
        {
            var engine = new MeetingRoomEngine();
            var conferenceRoomAccess = new ConferenceRoomAccess();

            Console.WriteLine($"Checking for rooms for {dateToCheck.ToString("f")}...");

            var meetingRooms = conferenceRoomAccess.GetMeetingRooms();
            var freeRooms = engine.GetFreeRooms(meetingRooms, dateToCheck);

            Console.WriteLine("Matching rooms:");
            if (!freeRooms.Any())
            {
                PromptAndWait("None.");
                return;
            }

            for (int i = 0; i < freeRooms.Count(); i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {Printer.PrintStatus(freeRooms[i].Name, dateToCheck, engine.GetFreeTimeAsOf(freeRooms[i], dateToCheck))}");
            }

            var roomNotChosen = true;
            while (roomNotChosen)
            {
                try
                {
                    var roomChoice = int.Parse(PromptAndWait("Select the room you want for the meeting:"));
                    Clipboard.SetText(freeRooms[roomChoice - 1].Name);
                    var duration = int.Parse(PromptAndWait("Please enter the duration in minutes:"));
                    conferenceRoomAccess.ScheduleMeeting(freeRooms[roomChoice - 1].Name, duration, dateToCheck);
                    roomNotChosen = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please choose a number from above");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Meeting Created!!");
        }


        private static string PromptAndWait(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}

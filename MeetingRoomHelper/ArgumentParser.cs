using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MeetingRoomHelper.Core;

namespace MeetingRoomHelper
{
    public class ArgumentParser
    {
        public List<DateTime> DateRangeToCheck { get; private set; }
        public DateTime DateToCheck { get; private set; }

        public bool PrintHelp { get; private set; }

        public bool ExactDate => DateRangeToCheck == null;

        public enum SearchScope
        {
            AllDay,
            Morning,
            Afternoon,
            Exact
        }

        public ArgumentParser(string[] args, DateTime currentDate)
        {
            PrintHelp = args.Any(a => a.Contains("?") || a.Contains("help") || a.Contains("man"));

            var i = 0;
            var date = currentDate;
            var hour = currentDate.Hour;
            var minute = currentDate.Minute;
            var scope = SearchScope.Exact;
            while (i < args.Length)
            {
                switch (args[i].ToLower())
                {
                    case "-d":
                    case "/d":
                        date = DateTime.Parse(args[i + 1]);
                        break;
                    case "-t":
                    case "/t":
                        if (args[i + 1].ToLower() == "afternoon")
                        {
                            scope = SearchScope.Afternoon;
                        }
                        else if (args[i + 1].ToLower() == "morning")
                        {
                            scope = SearchScope.Morning;
                        }
                        else if (args[i + 1].ToLower() == "allday")
                        {
                            scope = SearchScope.AllDay;
                        }
                        else
                        {
                            var split = args[i + 1].Split(':');
                            hour = int.Parse(split[0]);
                            minute = int.Parse(split[1]);
                        }
                        break;
                    default:
                        PrintHelp = true;
                        break;
                }
                i = i + 2;
            }
;
            if (scope == SearchScope.Exact)
            {
                DateToCheck = TimeHelper.AdjustToClosesTimeInterval(TimeHelper.CreateTime(date, hour, minute, 00));
            }
            else
            {
                DateRangeToCheck = CreateRange(date, scope);
            }
        }

        private List<DateTime> CreateRange(DateTime date, SearchScope scope)
        {
            var range = new List<DateTime>();
            var start = TimeHelper.CreateTime(date, 08, 00, 00);
            var finish = TimeHelper.CreateTime(date, 17, 00, 00);
            if (scope == SearchScope.Morning)
            {
                finish = TimeHelper.CreateTime(date, 12, 00, 00);
            }
            if (scope == SearchScope.Afternoon)
            {
                start = TimeHelper.CreateTime(date, 12, 00, 00);
            }
            while (start < finish)
            {
                range.Add(start);
                start = start.AddMinutes(TimeHelper.TimeInterval);
            }

            return range;
        }
    }
}
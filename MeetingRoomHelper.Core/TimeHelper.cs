using System;

namespace MeetingRoomHelper.Core
{
    public static class TimeHelper
    {
        public const int TimeInterval = 15;
        public const int MaxHour = 18;

        public static int SegmentsInDay => 24 * (60 / TimeInterval);

        public static DateTime CreateTime(int hour, int minute, int second)
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, second);
        }

        public static DateTime CreateTime(DateTime date, int hour, int minute, int second)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second);
        }

        public static DateTime ZeroOutSeconds(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
        }

        public static DateTime AdjustToClosesTimeInterval(DateTime nonAdjustedTime)
        {
            return ZeroOutSeconds(nonAdjustedTime.AddMinutes(-1 * (nonAdjustedTime.Minute - TimeInterval * (nonAdjustedTime.Minute / TimeInterval))));
        }
    }
}

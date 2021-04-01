using System;

namespace Jobz.WebUi.Utilities
{
    public static class DateUtil
    {
        public static DateTime LastWednesday()
        {
            var result = DateTime.UtcNow.AddHours(-7);//az
            while (result.DayOfWeek != DayOfWeek.Wednesday)
            {
                result = result.AddDays(-1);
            }
            return result.Date;
        }
    }
}
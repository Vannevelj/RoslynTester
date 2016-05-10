using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class DateTimeExtentions
    {
        public static DateTime AddPlusOneHour(this DateTime dt, TimeSpan value)
        {
            return dt.Add(value.Add(TimeSpan.FromHours(1)));
        }
    }
}

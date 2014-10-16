using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    class DateFunctions
    {
        public static Dictionary<int, List<DateTime>> HolidayCache = new Dictionary<int, List<DateTime>>();

        /// <summary>
        /// Takes a date in a specified format and adds a certain number of business days to it.
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="date">The input date</param>
        /// <param name="format">The input date format</param>
        /// <param name="days">Number of business days to add</param>
        /// <param name="guarantee_business_day">Makes sure that output date is a business day even if input date is not (effectively adds one to the offset if the input date is not a business day)</param>
        /// <returns>The new date</returns>
        /// <category>Date Manipulation</category>
        [MvmExport("add_business_days")]
        public static string add_business_days(ModuleContext _module_context, string date, string format, string days, string guarantee_business_day)
        {
            DateTime from = DateTime.ParseExact(date, format, null);
            int offset = days.ToInt();
            bool guarantee = guarantee_business_day.Equals("1");

            // Cache holiday info from the database
            object h;
            Dictionary<int, List<DateTime>> holidays;
            if (!_module_context.globalContext.GetNamedClassInst("MVM_HOLIDAYS", out h))
            {
                holidays = new Dictionary<int, List<DateTime>>();
                string calendar = _module_context.mvm.globalContext["mvm_holiday_calendar"];
                string globalTargetLoginOid = _module_context.mvm.globalContext["target_login"];
                StaticDbLoginInfo dbInfo = StaticDbLoginInfo.FromObjectId(_module_context.mvm, globalTargetLoginOid);
                string query = @"
                    select bp.nm_name as calendar_name, ch.nm_name as holiday_name, ch.n_day as day, ch.n_month as month, ch.n_year as year from t_base_props bp
                    inner join t_calendar c on c.id_calendar = bp.id_prop
                    inner join t_calendar_day cd on cd.id_calendar = c.id_calendar
                    inner join t_calendar_holiday ch on ch.id_day = cd.id_day
                    where bp.nm_name = '" + calendar + @"'
                    order by ch.n_year asc, ch.n_month asc, ch.n_day asc
                ";
                var results = DbUtils.DbQueryToListOfDictionary(dbInfo.type, dbInfo.server, dbInfo.db, dbInfo.user, dbInfo.pw, query);
                foreach (var row in results)
                {
                    int year = row["YEAR"].ToInt();
                    int month = row["MONTH"].ToInt();
                    int day = row["DAY"].ToInt();
                    holidays.GetAddValueDefaulted(MonthBucket(year, month), new List<DateTime>()).Add(new DateTime(year, month, day));
                }
                _module_context.globalContext.SetNamedClassInst("MVM_HOLIDAYS", holidays);
            }
            else
            {
                holidays = h as Dictionary<int, List<DateTime>>;
            }

            int increment = offset < 0 ? -1 : 1;
            if (guarantee && (isWeekend(from) || isHoliday(holidays, from)))
            {
                offset += increment;
            }

            while (offset != 0)
            {
                do
                {
                    from = from.AddDays(increment);
                }
                while (isWeekend(from) || isHoliday(holidays, from));
                offset -= increment;
            }
            return from.ToString(format);
        }

        public static int MonthBucket(int year, int month)
        {
            return year * 100 + month;
        }

        public static bool isWeekend(DateTime date)
        {
            return date.DayOfWeek.In(DayOfWeek.Saturday, DayOfWeek.Sunday);
        }

        public static bool isHoliday(Dictionary<int, List<DateTime>> holidays, DateTime date)
        {
            int key = MonthBucket(date.Year, date.Month);
            if (holidays.ContainsKey(key))
            {
                foreach (var holiday in holidays[key])
                {
                    if (holiday.Date == date.Date)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

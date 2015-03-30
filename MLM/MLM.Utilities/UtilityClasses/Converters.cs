using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using System.Collections.ObjectModel;
using MLM.Configuration;

namespace MLM.UtilityClasses
{
    /// <summary>
    /// A utility Converter class for doing any sort of conversions
    /// </summary>
    public class Converters
    {
        private const string defaultTimeZoneAppSetting = "DefaultTimeZone";
        private const string appSettingsSection = "appSettings";

        public static TimeZoneInfo GetDefaultTimeZoneInfo()
        {
            string defaultTimeZone = string.Empty;
            string userTimeZone = string.Empty;

            TimeZoneInfo defaultTimeZoneInfo = null;
            
            try
            {
                if (HttpContext.Current != null)
                {
                    defaultTimeZone = HttpContext.Current.Items["DefaultTimeZone"].ToString();
                    userTimeZone = HttpContext.Current.Items["UserTimeZone"].ToString();
                    defaultTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
                }
            }
            catch (TimeZoneNotFoundException timeZoneNotFoundException)
            {
                
                defaultTimeZoneInfo = null;
            }
            catch (InvalidTimeZoneException invalidTimeZoneException)
            {
                defaultTimeZoneInfo = null;
            }

            if (defaultTimeZoneInfo == null)
            {   //if defaultTimeZoneInfo is null then get timezone value from config
                defaultTimeZone = CustomConfigurationManager.GetValueFromSection(appSettingsSection, defaultTimeZoneAppSetting);
                defaultTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZone);
            }

            return defaultTimeZoneInfo;
        }

        /// <summary>
        /// Converts the passed date argument to the configured default time zone
        /// </summary>
        /// <param name="date">Date to be converted</param>
        /// <returns>Date converted to the default time zone</returns>
        public static DateTime ConvertToDefaultTimeZone(DateTime date)
        {
            TimeZoneInfo defaultTimeZoneInfo = GetDefaultTimeZoneInfo();
            TimeZoneInfo utcTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

            date = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(date, utcTimeZoneInfo, defaultTimeZoneInfo);
        }


        /// <summary>
        /// Converts the passed date argument to the configured default time zone
        /// </summary>
        /// <param name="date">Date to be converted</param>
        /// <returns>Date converted to the default time zone</returns>
        public static DateTime ConvertToUtc(DateTime date)
        {
            /*Specify Kind before converting to UTC*/
            date = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            TimeZoneInfo defaultTimeZoneInfo = GetDefaultTimeZoneInfo();
            TimeZoneInfo utcTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            
            return TimeZoneInfo.ConvertTime(date, defaultTimeZoneInfo, utcTimeZoneInfo);
        }

        /// <summary>
        /// Converts the passed catalog code to the catalog year
        /// </summary>
        /// <param name="catalogCode">Catalog code to be converted e.g UG12,UG89</param>
        /// <returns>Catalog Year e.g 2012, 1989</returns>
        public static string ConvertToCatalogYear(string catalogCode)
        {
            int getYear = 0;
            string catalogYear = string.Empty;

            if (!string.IsNullOrEmpty(catalogCode) && catalogCode != "OTHER")
            {
                getYear = Convert.ToInt32(catalogCode.Substring(2, 2));

                if (getYear > 70)
                {
                    catalogYear = "19" + getYear;
                }
                else if (getYear < 70)
                {

                    if (getYear == 0 || getYear <= 9)
                    {
                        catalogYear = "200" + getYear;
                    }
                    else
                    {
                        catalogYear = "20" + getYear;
                    }
                }
            }

            return catalogYear;
        }

        /// <summary>
        /// Converts the passed date argument to the configured default time zone
        /// </summary>
        /// <param name="date">Date to be converted</param>
        /// <returns>Date converted to the default time zone</returns>
        public static DateTime ConvertToMST(DateTime date)
        {
            var defaultTimeZone = CustomConfigurationManager.GetValueFromSection(appSettingsSection, defaultTimeZoneAppSetting);
            TimeZoneInfo defaultTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZone);
            TimeZoneInfo utcTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

            return TimeZoneInfo.ConvertTime(date, utcTimeZoneInfo, defaultTimeZoneInfo);
        }


        public static bool IsDST(DateTime date)
        {
            TimeZoneInfo defaultTimeZoneInfo = GetDefaultTimeZoneInfo();
            return defaultTimeZoneInfo.IsDaylightSavingTime(date);
        }

        public static IEnumerable<DateTime> GetDSTStartEnd(int year)
        {
            TimeZoneInfo defaultTimeZoneInfo = GetDefaultTimeZoneInfo();
            return GetTransitionTimes(year, defaultTimeZoneInfo);
        }


        private static IEnumerable<DateTime> GetTransitionTimes(int year, TimeZoneInfo timeZone)
        {
            List<DateTime> startEndtimes = new List<DateTime>();
            
            // Instantiate DateTimeFormatInfo object for month names
            DateTimeFormatInfo dateFormat = CultureInfo.CurrentCulture.DateTimeFormat;

            // Get and iterate time zones on local computer
            //ReadOnlyCollection<TimeZoneInfo> timeZones = defaultTimeZoneInfo;
            //foreach (TimeZoneInfo timeZone in timeZones)
            //{
                Console.WriteLine("{0}:", timeZone.StandardName);
                TimeZoneInfo.AdjustmentRule[] adjustments = timeZone.GetAdjustmentRules();
                int startYear = year;
                int endYear = startYear;

                if (adjustments.Length == 0)
                {
                    Console.WriteLine("   No adjustment rules.");
                }
                else
                {
                    TimeZoneInfo.AdjustmentRule adjustment = GetAdjustment(adjustments, year);
                    if (adjustment == null)
                    {
                        Console.WriteLine("   No adjustment rules available for this year.");
                        //continue;
                    }
                    TimeZoneInfo.TransitionTime startTransition, endTransition;

                    // Determine if starting transition is fixed 
                    startTransition = adjustment.DaylightTransitionStart;
                    // Determine if starting transition is fixed and display transition info for year 
                    if (startTransition.IsFixedDateRule)
                        startEndtimes.Add(new DateTime(startYear, startTransition.Month, startTransition.Day, startTransition.TimeOfDay.Hour, startTransition.TimeOfDay.Minute, startTransition.TimeOfDay.Second));
                    else
                        startEndtimes.Add(DisplayTransitionInfo(startTransition, startYear, "Begins on"));

                    //if (startTransition.IsFixedDateRule)
                    //    Console.WriteLine("   Begins on {0} {1} at {2:t}",
                    //                      dateFormat.GetMonthName(startTransition.Month),
                    //                      startTransition.Day,
                    //                      startTransition.TimeOfDay);
                    //else
                    //    DisplayTransitionInfo(startTransition, startYear, "Begins on");

                    // Determine if ending transition is fixed and display transition info for year
                    endTransition = adjustment.DaylightTransitionEnd;

                    // Does the transition back occur in an earlier month (i.e.,  
                    // the following year) than the transition to DST? If so, make 
                    // sure we have the right adjustment rule. 
                    if (endTransition.Month < startTransition.Month)
                    {
                        endTransition = GetAdjustment(adjustments, year + 1).DaylightTransitionEnd;
                        endYear++;
                    }

                    if (endTransition.IsFixedDateRule)
                        startEndtimes.Add(new DateTime(endYear, endTransition.Month, endTransition.Day, endTransition.TimeOfDay.Hour, endTransition.TimeOfDay.Minute, endTransition.TimeOfDay.Second));
                    else
                        startEndtimes.Add(DisplayTransitionInfo(endTransition, endYear, "Ends on"));

                    //if (endTransition.IsFixedDateRule)
                    //    Console.WriteLine("   Ends on {0} {1} at {2:t}",
                    //                      dateFormat.GetMonthName(endTransition.Month),
                    //                      endTransition.Day,
                    //                      endTransition.TimeOfDay);
                    //else
                    //    DisplayTransitionInfo(endTransition, endYear, "Ends on");
                }
            //}
                return startEndtimes;
        }


        private static TimeZoneInfo.AdjustmentRule GetAdjustment(TimeZoneInfo.AdjustmentRule[] adjustments,
                                                          int year)
        {
            // Iterate adjustment rules for time zone 
            foreach (TimeZoneInfo.AdjustmentRule adjustment in adjustments)
            {
                // Determine if this adjustment rule covers year desired 
                if (adjustment.DateStart.Year <= year && adjustment.DateEnd.Year >= year)
                    return adjustment;
            }
            return null;
        }

        private static DateTime DisplayTransitionInfo(TimeZoneInfo.TransitionTime transition, int year, string label)
        {
            // For non-fixed date rules, get local calendar
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            // Get first day of week for transition 
            // For example, the 3rd week starts no earlier than the 15th of the month 
            int startOfWeek = transition.Week * 7 - 6;
            // What day of the week does the month start on? 
            int firstDayOfWeek = (int)cal.GetDayOfWeek(new DateTime(year, transition.Month, 1));
            // Determine how much start date has to be adjusted 
            int transitionDay;
            int changeDayOfWeek = (int)transition.DayOfWeek;

            if (firstDayOfWeek <= changeDayOfWeek)
                transitionDay = startOfWeek + (changeDayOfWeek - firstDayOfWeek);
            else
                transitionDay = startOfWeek + (7 - firstDayOfWeek + changeDayOfWeek);

            // Adjust for months with no fifth week 
            if (transitionDay > cal.GetDaysInMonth(year, transition.Month))
                transitionDay -= 7;

            return new DateTime(year, transition.Month, transitionDay, transition.TimeOfDay.Hour, transition.TimeOfDay.Minute, transition.TimeOfDay.Second);

            //Console.WriteLine("   {0} {1}, {2:d} at {3:t}",
            //                  label,
            //                  transition.DayOfWeek,
            //                  new DateTime(year, transition.Month, transitionDay),
            //                  transition.TimeOfDay);
        }

        private const Int32 _InumberLength = 9;
        /// <summary>
        /// Function to format iNumber in the format ATI understands. Format is
        /// Length of INUmber should be of 9 characters. If less then left pad it with 0.
        /// </summary>
        /// <param name="iNumber"></param>
        /// <returns></returns>
        public static string GetFormattedINumber(string iNumber)
        {
            //If iNumber is empty or Length is as per ATI i.e. Length = 9 , then return the iNumber.
            if (string.IsNullOrWhiteSpace(iNumber) || iNumber.Length == _InumberLength)
            {
                return iNumber;
            }

            //If we reached here that means iNumber is not as per specified ATI length.
            //Get no of zero we need to pad the iNumber with.
            var noOfZeroPadding = _InumberLength - iNumber.Length;

            // pad iNUmber with 0s.
            for (int i = 0; i <= noOfZeroPadding - 1; i++)
            {
                iNumber = "0" + iNumber;
            }
            return iNumber;
        }

    }
}

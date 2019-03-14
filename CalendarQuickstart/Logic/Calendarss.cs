using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarQuickstart.Logic
{
    class Calendarss
    {
        static CalendarService service;

        public Calendarss()
        {
            service = GService.service;
        }

        public static Calendar getCalender(Calendar calenderToGet)
        {

            Calendar calander = service.Calendars.Get(calenderToGet.Id).Execute();

            return calander;
        }
        public static Calendar getCalenderById(String id)
        {

            Calendar calander = service.Calendars.Get(id).Execute();

            return calander;
        }
        public static CalendarListEntry getCalenderByName(String name)
        {
            //this is not with type Calander, make sure to use var or CalendarListEntry when using

            var calenders = getCalenders();

            foreach (CalendarListEntry cal in calenders)
            {

                if (cal.Summary == name)
                {
                    return cal;
                }
            }

            return null;
        }

        public static IList<CalendarListEntry> getCalenders()
        {

            var calenders = service.CalendarList.List().Execute().Items;

            return calenders;

        }

        public static CalendarsResource.InsertRequest newCalendar(string name, string location, string timeZone = "America/Los_Angeles")
        {

            Calendar newCalendar = new Calendar
            {
                Summary = name,
                Location = location,
                TimeZone = timeZone


            };

            //probably not used...
            return service.Calendars.Insert(newCalendar);

        }

        public static void updateCalendarById(string id, string newName, string newLocation = "unknown", string newDescription = "none", string newTimeZone = "America/Los_Angeles")
        {

            Calendar calendar = service.Calendars.Get(id).Execute();


            // Make a change
            calendar.Summary = "new Summary";
            calendar.Location = newName;
            calendar.Location = newLocation;
            calendar.Description = newDescription;

            // Update the altered calendar
            service.Calendars.Update(calendar, calendar.Id).Execute();

        }
        public static void updateCalendar(Calendar calendar, string newName, string newLocation = "unknown", string newDescription = "none", string newTimeZone = "America/Los_Angeles")
        {
            // Make a change
            calendar.Summary = "new Summary";
            calendar.Location = newName;
            calendar.Location = newLocation;
            calendar.Description = newDescription;

            // Update the altered calendar
            service.Calendars.Update(calendar, calendar.Id).Execute();

        }
        //not tested!!!
        public static void updateCalendarByName(string oldName, string newName, string newLocation = "unknown", string newDescription = "none", string newTimeZone = "America/Los_Angeles")
        {

            var calenders = getCalenders();
            CalendarListEntry foundCalendar = null;

            foreach (CalendarListEntry cal in calenders)
            {

                if (cal.Summary == oldName)
                {
                    foundCalendar = cal;
                }
            }

            // Make a change
            Calendar calender = null;
            calender.Summary = newName;
            calender.Location = newLocation;
            calender.Description = newDescription;
            calender.TimeZone = newTimeZone;
            calender.Id = foundCalendar.Id;

            // Update the altered calendar
            service.Calendars.Update(calender, foundCalendar.Id).Execute();

        }

        public static void deleteCalendar(Calendar calendar)
        {

            service.Calendars.Delete(calendar.Id).Execute();

        }
        public static void deleteCalendarById(string id)
        {

            service.Calendars.Delete(id).Execute();

        }
        public static void deleteCalendarByName(string name)
        {

            var calenders = getCalenders();
            CalendarListEntry foundCalander = null;

            foreach (CalendarListEntry cal in calenders)
            {

                if (cal.Summary == name)
                {
                    foundCalander = cal;
                }
            }
            service.Calendars.Delete(foundCalander.Id).Execute();

        }
    }
}

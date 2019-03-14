using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarQuickstart.Logic
{
    class Eventss
    {
        public static CalendarService service;


        public Eventss()
        {
            service = GService.service;
        }

        //in construction
        public static Events getEvents()
        {



            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
            return events;
        }

        public static Events getAllnotDeletedEvents()
        {

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            return events;

            /**
			String pageToken = null;
			do
			{
				Events events = service.Events.List('primary').setPageToken(pageToken).execute();
				IList<Event> items = events.Items;
			
				pageToken = events.NextPageToken;
			} while (pageToken != null);	*/

        }
        public static Events getAllDeletedEvents()
        {
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = true;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();

            return events;
        }

        public static Event getEventById(string eventId, string calendarId = "primary")
        {
            Event ev = service.Events.Get(calendarId, eventId).Execute();

            return ev;

        }
        public static Dictionary<Event, Boolean> getEventByName(string name, string calendarId = "primary")
        {
            Events eventsNotDeleted = getAllnotDeletedEvents();
            foreach (Event ev in eventsNotDeleted.Items)
            {
                if (ev.Summary == name)
                {
                    Dictionary<Event, Boolean> returnvalue = null;
                    returnvalue.Add(ev, true);

                    return returnvalue;

                }
            }


            Events eventsDeleted = getAllDeletedEvents();
            foreach (Event ev in eventsDeleted.Items)
            {
                if (ev.Summary == name)
                {
                    Dictionary<Event, Boolean> returnvalue = null;
                    returnvalue.Add(ev, false);

                    return returnvalue;

                }
            }

            return null;


        }
        public static Event getEvent(Event eventToFind, string calendarId = "primary")
        {
            Event ev = service.Events.Get(calendarId, eventToFind.Id).Execute();

            return ev;
        }

        //usable but recurrence of event and attendees are hardcoded, later update
        public static void newEvent(string summary, string location, string description, DateTime startDate, DateTime endDate, List<EventAttendee> eventAttendees, String TimeZone = "America/Los_Angeles", string calendarId = "primary")
        {
            Event myEvent = new Event
            {
                Summary = summary,
                Location = location,
                Description = description,
                Start = new EventDateTime()
                {
                    DateTime = startDate,
                    TimeZone = TimeZone
                },
                End = new EventDateTime()
                {
                    DateTime = endDate,
                    TimeZone = TimeZone
                },

                Attendees = eventAttendees

            };

            Event newEvent = service.Events.Insert(myEvent, calendarId).Execute();
            addAttendees(eventAttendees, summary);
        }
        public static void addAttendees(List<EventAttendee> eventAttendees, string summary)
        {


            Dictionary<Event, Boolean> events = getEventByName(summary);
            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {

                foreach (Event ev in events.Keys)
                {
                    foreach (EventAttendee att in ev.Attendees)
                    {
                        eventAttendees.Add(att);
                    }
                    ev.Attendees = eventAttendees;
                    eventToUpdate = ev;
                }


            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event to add attendees");
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, "primary", eventToUpdate.Id).Execute();

        }


        //whole block usable but very basic...
        public static void updateEvent(Event eventToUpdate)
        {

            Event ev = service.Events.Get("primary", eventToUpdate.Id).Execute();

            //make a change
            ev.Summary = "demo new event name";

            // Update the event
            Event updatedEvent = service.Events.Update(ev, "primary", eventToUpdate.Id).Execute();
        }
        public static void DeleteEvent(Event eventToDelete, string calendarId = "primary")
        {
            Console.WriteLine("test123");
            service.Events.Delete(calendarId, eventToDelete.Id).Execute();
            Console.WriteLine("test");
        }
        public static void archiveEvent(Event eventToArchive)
        {
            Event test = service.Events.Get("primary", eventToArchive.Id).Execute();

            //make a change
            test.Locked = true;


            // Update the event
            Event updatedEvent = service.Events.Update(test, "primary", eventToArchive.Id).Execute();


        }
        public static void unarchiveEvent(Event eventToUnarchive)
        {

            Event test = service.Events.Get("primary", eventToUnarchive.Id).Execute();

            //make a change
            test.Locked = false;


            // Update the event
            Event updatedEvent = service.Events.Update(test, "primary", eventToUnarchive.Id).Execute();



        }
    }
}

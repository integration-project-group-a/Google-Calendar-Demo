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

        public static Events getAllEvents(string calendarId = "primary")
        {
            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List(calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = true;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            return events;
        }
        public static Events getAllnotDeletedEvents(string calendarId = "primary")
        {
            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List(calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            return events;       
        }
        public static Events getAllDeletedEvents(string calendarId = "primary")
        {
            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List(calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = true;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

            foreach (Event ev in events.Items) {
                //tests if the event has not been deleted
                if (ev.Status != "cancelled") {
                    events.Items.Remove(ev);
                }

            }
            return events;
        }

        //boolean? event excist: event is deleted / if return of methode is null event doesn't excist
        public static Dictionary<Event, Boolean> getEventById(string eventId, string calendarId = "primary")
        {
            Events events = getAllEvents();
            foreach (Event ev in events.Items)
            {
                if (ev.Status != "canceled")
                {
                    if (ev.Id == eventId)
                    {
                        Dictionary<Event, Boolean> returnvalue = null;
                        returnvalue.Add(ev, true);

                        return returnvalue;
                    }
                }
                else
                {
                    if (ev.Id == eventId)
                    {
                        Dictionary<Event, Boolean> returnvalue = null;
                        returnvalue.Add(ev, false);

                        return returnvalue;
                    }
                }
            }
            return null;
        }
        public static Dictionary<Event, Boolean> getEventByName(string name, string calendarId = "primary")
        {
            Events events = getAllEvents();
            foreach (Event ev in events.Items)
            {
                if (ev.Status != "canceled")
                {
                    if (ev.Summary == name)
                    {
                        Dictionary<Event, Boolean> returnvalue = null;
                        returnvalue.Add(ev, true);

                        return returnvalue;
                    }
                }
                else {
                    if (ev.Summary == name)
                    {
                        Dictionary<Event, Boolean> returnvalue = null;
                        returnvalue.Add(ev, false);

                        return returnvalue;
                    }
                }         
            }

            return null;
        }
        public static Dictionary<Event, Boolean> getEvent(Event eventToFind, string calendarId = "primary")
        {
            Events events = getAllEvents();
            foreach (Event ev in events.Items)
            {
                if (ev.Status != "canceled")
                {
                    if (ev.Id == eventToFind.Id)
                    {
                        Dictionary<Event, Boolean> returnvalue = null;
                        returnvalue.Add(ev, true);

                        return returnvalue;
                    }
                }
                else
                {
                    if (ev.Id == eventToFind.Id)
                    {
                        Dictionary<Event, Boolean> returnvalue = null;
                        returnvalue.Add(ev, false);

                        return returnvalue;
                    }
                }
            }

            return null;
        }

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
        }

        public static void addAttendeesByName(List<EventAttendee> eventAttendees, string name, string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEventByName(name);
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

            Event updatedEvent = service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();
        }
        public static void addAttendeesById(List<EventAttendee> eventAttendees, string eventId, string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEventById(eventId);
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

            Event updatedEvent = service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();   
        }
        public static void addAttendees(List<EventAttendee> eventAttendees, Event eventToFind, string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEvent(eventToFind);
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

            Event updatedEvent = service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();
        }

        //these methodes will overrride predeclared values, example: eventAttendees will overide the old attendees not add them to the excisting ones
        //do not change calendarId, you can't update it
        //RESEARCH NEEDED to update calendar id
        public static void updateEventByName(string oldSummery, string newSummary, string location, string description, DateTime startDate, DateTime endDate, List<EventAttendee> eventAttendees, String TimeZone = "America/Los_Angeles", string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEventByName(oldSummery);

            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {
                foreach (Event ev in events.Keys)
                {
                    ev.Summary = newSummary;
                    ev.Location = location;
                    ev.Description = description;
                    ev.Start = new EventDateTime()
                    {
                        DateTime = startDate,
                        TimeZone = TimeZone
                    };
                    ev.End = new EventDateTime()
                    {
                        DateTime = endDate,
                        TimeZone = TimeZone
                    };
                    ev.Attendees = eventAttendees;
                    eventToUpdate = ev;
                }
            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event before updating it");
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, "primary", eventToUpdate.Id).Execute();
        }
        public static void updateEventById(string eventId, string summary, string location, string description, DateTime startDate, DateTime endDate, List<EventAttendee> eventAttendees, String TimeZone = "America/Los_Angeles", string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEventById(eventId);

            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {
                foreach (Event ev in events.Keys)
                {
                    ev.Summary = summary;
                    ev.Location = location;
                    ev.Description = description;
                    ev.Start = new EventDateTime()
                    {
                        DateTime = startDate,
                        TimeZone = TimeZone
                    };
                    ev.End = new EventDateTime()
                    {
                        DateTime = endDate,
                        TimeZone = TimeZone
                    };
                    ev.Attendees = eventAttendees;
                    eventToUpdate = ev;
                }
            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event before updating it");
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, "primary", eventToUpdate.Id).Execute();
        }
        public static void updateEvent(Event eventUpdate, string summary, string location, string description, DateTime startDate, DateTime endDate, List<EventAttendee> eventAttendees, String TimeZone = "America/Los_Angeles", string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEvent(eventUpdate);

            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {
                foreach (Event ev in events.Keys)
                {
                    ev.Summary = summary;
                    ev.Location = location;
                    ev.Description = description;
                    ev.Start = new EventDateTime()
                    {
                        DateTime = startDate,
                        TimeZone = TimeZone
                    };
                    ev.End = new EventDateTime()
                    {
                        DateTime = endDate,
                        TimeZone = TimeZone
                    };
                    ev.Attendees = eventAttendees;
                    eventToUpdate = ev;
                }
            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event before updating it");
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, "primary", eventToUpdate.Id).Execute();
        }

        //you can recover "deleted" events
        //RESARCH NEEDED to know how long they will be able to be recovered
        public static void DeleteEvent(Event eventToDelete, string calendarId = "primary")
        {
            service.Events.Delete(calendarId, eventToDelete.Id).Execute();
        }
        public static void UnDeleteEvent(Event eventToArchive, string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEvent(eventToArchive);

            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {
                foreach (Event ev in events.Keys)
                {
                    ev.Status = "confirmed";
                    eventToUpdate = ev;
                }
            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event before updating it");
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();
        }
        public static void UnDeleteEventByName(string name, string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEventByName(name);

            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {
                foreach (Event ev in events.Keys)
                {
                    ev.Status = "confirmed";
                    eventToUpdate = ev;
                }
            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event before updating it");
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();
        }
        public static void UnDeleteEventById(string eventId, string calendarId = "primary")
        {
            Dictionary<Event, Boolean> events = getEventById(eventId);

            Event eventToUpdate = null;
            if (events.ContainsValue(true))
            {
                foreach (Event ev in events.Keys)

                {
                    ev.Status = "confirmed";
                    eventToUpdate = ev;
                }
            }
            if (events.ContainsValue(false))
            {
                Console.Write("Event is marked as inactive, activate event before updating it");
                return;
            }
            if (events == null)
            {
                Console.Write("Event doesn't excist");
                return;
            }

            Event updatedEvent = service.Events.Update(eventToUpdate, calendarId, eventToUpdate.Id).Execute();
        }
    }
}

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarQuickstart
{

	public class GService
	{
		static string ApplicationName = "Google Calendar API .NET Quickstart";
		string[] Scopes = { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents
		};

		public static CalendarService service { get; set; }

		public GService() {
			UserCredential credential;

			using (var stream =
				new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				// The file token.json stores the user's access and refresh tokens, and is created
				// automatically when the authorization flow completes for the first time.
				string credPath = "token.json";
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"admin",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			GService.service = new CalendarService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

		}
			
	}

	public class Calendarss
	{
		static CalendarService service;

		public Calendarss()
		{
			service = GService.service;
		}

		public static Calendar getCalender(Calendar calenderToGet) {

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

		public static IList<CalendarListEntry> getCalenders() {

			var calenders = service.CalendarList.List().Execute().Items;

			return calenders;

		}
			
		public static CalendarsResource.InsertRequest newCalendar(string name, string location, string timeZone = "America/Los_Angeles")
		{

			Calendar newCalendar = new Calendar {
				Summary = name,
				Location = location,
				TimeZone = timeZone


			};

			//probably not used...
			return service.Calendars.Insert(newCalendar);

		}

		public static void updateCalendarById(string id,  string newName, string newLocation = "unknown", string newDescription = "none", string newTimeZone = "America/Los_Angeles") {

			Calendar calendar = service.Calendars.Get(id).Execute();


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

		public static void deleteCalendar(Calendar calendar) {

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

	public class Eventss
	{
		public static CalendarService  service;


		public Eventss()
		{
			service = GService.service;
		}

		public static Events getevents()
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

		public static void getEvent(Event eventToFind, string calendarId = "primary")
		{
			service.Events.Get(calendarId, eventToFind.Id).Execute();
		}

		public static void newEvent()
		{
			Event myEvent = new Event
			{
				Summary = "demo event test",
				Location = "Somewhere",
				Start = new EventDateTime()
				{
					DateTime = new DateTime(2019, 8, 3, 10, 0, 0),
					TimeZone = "America/Los_Angeles"
				},
				End = new EventDateTime()
				{
					DateTime = new DateTime(2019, 8, 3, 10, 30, 0),
					TimeZone = "America/Los_Angeles"
				},
				Recurrence = new String[] {
	  "RRULE:FREQ=WEEKLY;BYDAY=MO"
  },
				Attendees = new List<EventAttendee>()
	  {
		new EventAttendee() { Email = "johndoe@gmail.com" }
	  }
			};

			Event recurringEvent = service.Events.Insert(myEvent, "primary").Execute();

		}

		public static void updateEvent(Event eventToUpdate) {

		Event test = service.Events.Get("primary", eventToUpdate.Id).Execute();

			//make a change
			test.Summary = "demo new event name";
		

		// Update the event
		Event updatedEvent = service.Events.Update(test, "primary", eventToUpdate.Id).Execute();
	

		}


		public static void DeleteEvent(Event eventToDelete, string calendarId = "primary")
		{
			Console.WriteLine("test123");
			service.Events.Delete(calendarId, eventToDelete.Id).Execute();
			Console.WriteLine("test");
		}


		public static void archiveEvent(Event eventToArchive) {
			Event test = service.Events.Get("primary", eventToArchive.Id).Execute();

			//make a change
			test.Locked = true;


			// Update the event
			Event updatedEvent = service.Events.Update(test, "primary", eventToArchive.Id).Execute();


		}


		public static void unarchiveEvent(Event eventToUnarchive) {

			Event test = service.Events.Get("primary", eventToUnarchive.Id).Execute();

			//make a change
			test.Locked = false;


			// Update the event
			Event updatedEvent = service.Events.Update(test, "primary", eventToUnarchive.Id).Execute();



		}
	}
	class Program
	{


		 static void Main(string[] args) {

			
		}
			

	}

	}


	

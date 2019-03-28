using System;
using System.IO;
using System.Threading;
using Google;
using System.Collections.Generic;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;

namespace CalendarQuickstart.Logic
{
	//contacts
	public class Attendee
	{
		static string ApplicationName = "Google Calendar API .NET Quickstart";
		string[] Scopes = { PeopleService.Scope.Contacts};
		public static PeopleService  peopleService;
		PeopleService Service;


		public Attendee()
		{

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

			// Create the service.
			peopleService = new PeopleService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

		}
		public Attendee(string test)
		{

			// Create OAuth credential.
			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				new ClientSecrets
				{
					ClientId = "796797994752-otivl19j162n9mhgvd56pahdg77cli54.apps.googleusercontent.com",
					ClientSecret = "uVb1zDwcFc7vY29TtVQG_VnS"
				},
				new[] { "profile", "https://www.googleapis.com/auth/contacts" },
				"me",
				CancellationToken.None).Result;

			// Create the service.
			Service = new PeopleService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "APP_NAME",
			});
		}
			//contacts
			public  bool createAttendees(string voornaam, string achternaam, string uuid)
		{


			PeopleResource.ConnectionsResource.ListRequest peopleRequest = peopleService.People.Connections.List("people/me");

			peopleRequest.RequestMaskIncludeField = new List<String>() { "person.addresses", "person.names" };
			ListConnectionsResponse people = peopleRequest.Execute();
		



			peopleRequest.Execute();

			foreach (var per in people.Connections)
			{
				Console.Write(per.Names[0].DisplayName);
			}
			return true;
		}

		//with uuid
		public bool updateAttendees(string uuid)
		{
			//search in contact for contact with uuid^^
			//update name, mail

			if (true)
			{
				return true;
			}
			else return false;

		}
		public void getAll() {
		
		}
	



	}
}

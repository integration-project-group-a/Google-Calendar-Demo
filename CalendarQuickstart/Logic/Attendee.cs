using Google.Apis.Auth.OAuth2;
using Google.Apis.People.v1;
using Google.Apis.People.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarQuickstart.Logic
{
	//contacts
	public class Attendee
	{
		static string ApplicationName = "Google Calendar API .NET Quickstart";
		string[] Scopes = { Google.Apis.People.v1.PeopleService.Scope.Contacts, Google.Apis.People.v1.PeopleService.Scope.ContactsReadonly, Google.Apis.People.v1.PeopleService.Scope.PlusLogin, Google.Apis.People.v1.PeopleService.Scope.UserinfoProfile};
		public static Google.Apis.People.v1.PeopleService peopleService;
		//PeopleService Service;


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
			peopleService = new Google.Apis.People.v1.PeopleService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

		}
		public Attendee(string test) {
			// Create OAuth credential.
			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				
				new ClientSecrets
				{
					ClientId = "796797994752-otivl19j162n9mhgvd56pahdg77cli54.apps.googleusercontent.com",
					ClientSecret = "uVb1zDwcFc7vY29TtVQG_VnS"
				},
				new[] { "profile", "https://www.googleapis.com/auth/contacts.readonly" },
				"me",
				CancellationToken.None).Result;

			// Create the service.
			peopleService = new PeopleService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "APP_NAME",
			});

		}

		/**
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
	*/
		//contacts
		public Google.Apis.PeopleService.v1.Data.Person createAttendees(string voornaam, string middleName, string achternaam, string uuid, string email)
		{
			Google.Apis.PeopleService.v1.Data.Person contactToCreate = new Google.Apis.PeopleService.v1.Data.Person();
			List<Google.Apis.PeopleService.v1.Data.Name> names = new List<Google.Apis.PeopleService.v1.Data.Name>();
			names.Add(new Google.Apis.PeopleService.v1.Data.Name() { GivenName = voornaam, FamilyName = achternaam, MiddleName = middleName });
			
			List<Google.Apis.PeopleService.v1.Data.EmailAddress> emailAddresses = new List<Google.Apis.PeopleService.v1.Data.EmailAddress>() ;
			emailAddresses.Add(new Google.Apis.PeopleService.v1.Data.EmailAddress() { Value = email });

			List<Google.Apis.PeopleService.v1.Data.UserDefined> uuids = new List<Google.Apis.PeopleService.v1.Data.UserDefined>();
			uuids.Add(new Google.Apis.PeopleService.v1.Data.UserDefined() { Value = uuid, Key = "UUID" });

			contactToCreate.Names = names;
			contactToCreate.EmailAddresses = emailAddresses;
			contactToCreate.UserDefined = uuids;

			Google.Apis.PeopleService.v1.PeopleResource.CreateContactRequest request =
			 new Google.Apis.PeopleService.v1.PeopleResource.CreateContactRequest(peopleService, contactToCreate);
			Google.Apis.PeopleService.v1.Data.Person createdContact = request.Execute();
			return createdContact;

		}

		public void getPerson()
		{

			PeopleResource.ConnectionsResource.ListRequest peopleRequest =
			peopleService.People.Connections.List("people/me");
			peopleRequest.RequestMaskIncludeField = "person.names";
			ListConnectionsResponse connectionsResponse = peopleRequest.Execute();
			IList<Person> connections = connectionsResponse.Connections;

			foreach (var test in connections) {

				Console.Write(test.Names.First().DisplayName);
				Console.Read();

					}
		}

		//with uuid
	//	public bool updateAttendees(string uuid)
		public void getAll() {

			
		}
	

	

	}
}

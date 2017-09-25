using System;
using System.Collections.Generic;
using CalendarsTester.Core.Services;
using CalendarsTester.Services;
using Xamarin.Forms;
using CalendarsTester.Helpers;
using CalendarsTester.Core.ViewModels;
using CalendarsTester.Pages;
using CalendarsTester.Core.Helpers;
using System.IO;
using System.Net.Http;
using System.Net;

namespace CalendarsTester
{
    //Login Page with option for the user to set a passcode
    public partial class Login : ContentPage
    {

        //variables to store passcode
        public string userPasscodeValue { get; set; }
        public bool isPassCodeFound { get; set; }

        public Login()
        {
            InitializeComponent();

			userPasscodeValue = null;
        }

		// Encryption Interface which has the encryption implementation in Android(MainActivity.cs) and IOS(Main.cs) files
		//https://forums.xamarin.com/discussion/50703/how-to-use-sha-hash-in-xamarin
		public interface iSHA512StringHash
		{
			string SHA512StringHash(string input);
		}

        //Method to execute after Login button clicked.
		private async void loginMethod(object sender, System.EventArgs e)
        {
            
            DependencyService.Register<IReportingService, ReportingService>();
            DependencyService.Register<ViewProvider>();

			ActivityIndicator spinner = this.FindByName<ActivityIndicator>("ActivitySpinner");

			spinner.IsVisible = true;
			spinner.IsRunning = true;


            checkIfPassCodeExists();

        }

        private async System.Threading.Tasks.Task movetoHomeScreen()
        {
            viewRegistration();

            var viewProvider = DependencyService.Get<ViewProvider>();

            await Navigation.PushAsync(viewProvider.GetView(ViewModelProvider.GetViewModel<CalendarsViewModel>()) as Page);
        }

        private void viewRegistration()
		{
			var viewProvider = DependencyService.Get<ViewProvider>();

			viewProvider.Register<CalendarsViewModel, CalendarsPage>();
			viewProvider.Register<CalendarEditorViewModel, CalendarEditorPage>();
			viewProvider.Register<DateTimeRangeViewModel, DateTimeRangePage>();
			viewProvider.Register<EventsViewModel, EventsPage>();
			viewProvider.Register<EventEditorViewModel, EventEditorPage>();
			viewProvider.Register<ReminderEditorViewModel, ReminderEditorPage>();
		}

		// Method to upload the passcode to server as well as to store it locally.
		public async void uploadUserPassCode()
		{
			if (Entry_UserPasscode != null)
			{
				// Encrypt password 
				userPasscodeValue = Entry_UserPasscode.Text;

				System.Diagnostics.Debug.WriteLine("Plain passcode : " + userPasscodeValue);

				string encryptedPassCode = DependencyService.Get<iSHA512StringHash>().SHA512StringHash(userPasscodeValue);

				System.Diagnostics.Debug.WriteLine("Encrypted passcode is : " + encryptedPassCode);

                //Save the passcode both locally for offline validation as well as send passcode to server
                Application.Current.Properties["Passcode"] = encryptedPassCode;
			
                // Send http request to server to store the user passcode
				var httpClient = new HttpClient();
                var objectID = "212529407_Passcode";
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://introtoapps.com/datastore.php?action=save&appid=212529407&objectid=" + objectID + "&data=" + encryptedPassCode);
				var response = await httpClient.SendAsync(request);
			}

            await movetoHomeScreen();
		}

        //Method to check if the passcode exists already or not 
        public async void checkIfPassCodeExists(){

            string encryptedPassCode = string.Empty;
            string responseStringPassCode = string.Empty;

			try
			{
				//Convert plain text to encrypted passcode
				// Encrypt password 
				userPasscodeValue = Entry_UserPasscode.Text;
				encryptedPassCode = DependencyService.Get<iSHA512StringHash>().SHA512StringHash(userPasscodeValue);

				// Send http request to check if a matching passcode exists
                var objectID = "212529407_Passcode";
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://introtoapps.com/datastore.php?action=load&appid=212529407&objectid=" + objectID + "&data=" + encryptedPassCode);
				webRequest.Method = "GET";

				HttpWebResponse httpResponse = (System.Net.HttpWebResponse)await webRequest.GetResponseAsync();

				using (StreamReader responseReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					responseStringPassCode = responseReader.ReadToEnd();
				}

                //Check the returned value with the encrypted value of passcode entered by user
                //If match found then move to home screen
                //If match is found with server value, also check with locally saved value
                //IF no match found then inform user and upload new passcode to server and to local storage
                if(responseStringPassCode.Equals(encryptedPassCode)){
					if (Application.Current.Properties.ContainsKey("Passcode"))
					{
						Application.Current.Properties["Passcode"] = encryptedPassCode;
						var localSavedPasscode = Application.Current.Properties["Passcode"] as String;
						if (localSavedPasscode.Equals(encryptedPassCode))
						{
							isPassCodeFound = true;
							//await DisplayAlert("Alert", "Passcode match true + " + responseStringPassCode + "--encryptedpasscode : " + encryptedPassCode, "OK");
                            await DisplayAlert("Alert", "Passcode Verification sucessful", "Ok");
							await movetoHomeScreen();
						}
					}

                } else
                {
                    await handlePasswordMismatach(encryptedPassCode, responseStringPassCode);
                }



            }
			catch (System.Net.WebException)
			{
                await handlePasswordMismatach(encryptedPassCode, responseStringPassCode);
			}
        }

        //Handle scenario when no matching passcode is found. Upload to server and save locally
        private async System.Threading.Tasks.Task handlePasswordMismatach(string encryptedPassCode, string respString)
        {
            isPassCodeFound = false;
            //await DisplayAlert("Alert", "Passcode match false + " + respString + "--encryptedpasscode : " + encryptedPassCode, "OK");
            await DisplayAlert("Alert", "This Passcode is not present in backend. The new passcode entered now will be stored in the backend system", "OK");
            uploadUserPassCode();
        }
    }
}


using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using System.Security.Cryptography;
using System.Text;
using static CalendarsTester.Login;
using static CalendarsTester.Droid.MainActivity;

[assembly: Xamarin.Forms.Dependency(typeof(SHA512HashImpl))]
namespace CalendarsTester.Droid
{
    [Activity(Label = "CalendarsTester", Icon = "@drawable/icon", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

		//Implementation as per https://forums.xamarin.com/discussion/50703/how-to-use-sha-hash-in-xamarin
		public class SHA512HashImpl : iSHA512StringHash
		{
			public SHA512HashImpl() { }

			public string SHA512StringHash(string input)
			{
				SHA512 shaM = new SHA512Managed();
				// Convert the input string to a byte array and compute the hash.
				byte[] data = shaM.ComputeHash(Encoding.UTF8.GetBytes(input));
				// Create a new Stringbuilder to collect the bytes
				// and create a string.
				StringBuilder sBuilder = new StringBuilder();
				// Loop through each byte of the hashed data 
				// and format each one as a hexadecimal string.
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
				// Return the hexadecimal string.
				input = sBuilder.ToString();
				return (input);
			}
		}
    }
}


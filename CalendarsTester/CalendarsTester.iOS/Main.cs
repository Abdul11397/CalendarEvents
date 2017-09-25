using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Foundation;
using UIKit;
using static CalendarsTester.iOS.Application;
using static CalendarsTester.Login;

// Dependeny link created for interfaces
[assembly: Xamarin.Forms.Dependency(typeof(SHA512HashImpl))]
namespace CalendarsTester.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
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

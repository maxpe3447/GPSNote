//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin.Auth;

//namespace GPSNote.Services.GoogleAuth
//{
//    public class GoogleAuth
//    {
//        // you can find following readonly strings in Constants.cs file in the 
//        //github project
//        // Pasted it here for ease of reference with the code
//        public static readonly string GOOGLE_ID =
//                                  "385305440253-r44fu0mjbees9e7bim3pu3usv0522f1d.apps.googleusercontent.com";
//        public static readonly string GOOGLE_SCOPE =
//                                 "https://www.googleapis.com/auth/userinfo.email";
//        public static readonly string GOOGLE_AUTH =
//                                   "https://accounts.google.com/o/oauth2/auth";
//        public static readonly string GOOGLE_REDIRECTURL =
//                                  "https://www.googleapis.com/plus/v1/people/me";
//        public static readonly string GOOGLE_REQUESTURL =
//                                  "https://www.googleapis.com/oauth2/v2/userinfo";


//        //googleBtn.Click += GoogleAuthentication;
//public static void GoogleAuthentication(/*object sender, EventArgs e*/)
//        {
//            var auth = new OAuth2Authenticator
//                       (
//                         GOOGLE_ID,
//                         GOOGLE_SCOPE,
//                         new Uri(GOOGLE_AUTH),
//                         new Uri(GOOGLE_REDIRECTURL)
//                        );

//            //Allowing user to cancel authentication if he want to..
//            auth.AllowCancel = true;

//            ////Used to launch the corresponding social Login page
//            ////StartActivity(auth.GetUI(this));

//            ////It will fire Once we are done with authentication
//            auth.Completed += async (sender, e) =>
//            {
//                //Fires when authentication is cancelled
//                if (!e.IsAuthenticated)
//                {
//                    //Authentication failed Do something
//                    return;
//                }
//                //Make request to get the parameters access
//                var request = new OAuth2Request
//                              (
//                                 "GET",
//                                  new Uri(GOOGLE_REQUESTURL),
//                                  null,
//                                  e.Account
//                              );
//                //Get response here
//                var response = await request.GetResponseAsync();
//                if (response != null)
//                {
//                    //Get the user data here
//                    var userData = response.GetResponseText();
//                }
//            };
//        }
//    }
//}

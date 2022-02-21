using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Prism;
using Prism.Ioc;
using Android;
using Android.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps.Android;

namespace GPSNote.Droid
{//, Theme = "@style/MainTheme"
    [Activity(Label = "GPSNote", Icon = "@mipmap/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };
            Xamarin.FormsGoogleMaps.Init(this,
                                         savedInstanceState,
                                         config: platformConfig);


            UserDialogs.Init(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            base.OnStart();
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                //if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted ||
                //    CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Permission.Granted ||
                //    CheckSelfPermission(Manifest.Permission.Internet) != Permission.Granted ||
                //    CheckSelfPermission(Manifest.Permission.AccessNetworkState) != Permission.Granted ||
                //    CheckSelfPermission(Manifest.Permission.ControlLocationUpdates) != Permission.Granted)
                //{
                //    RequestPermissions(LocationPermissions, RequestLocationId);
                //}

                //else
                //{
                //    Acr.UserDialogs.UserDialogs.Instance.Alert("all good");
                //    // Permissions already granted - display a message.
                //}

                var status =  CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location).Result;
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if ( CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location).Result)
                    {
                         Acr.UserDialogs.UserDialogs.Instance.Alert("Need location", "Gunna need that location", "OK");
                    }

                    var results =  CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Location });
                    status = results.Result[Plugin.Permissions.Abstractions.Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                }
                else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    if (requestCode == RequestLocationId)
        //    {
        //        if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
        //            Console.WriteLine("Location permissions granted.");
        //        else
        //            Console.WriteLine("Location permissions denied.");
        //    }
        //    else
        //    {
        //        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    }
        //}
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.Internet,
                Manifest.Permission.AccessNetworkState,
                Manifest.Permission.ControlLocationUpdates,
                Manifest.Permission.AccessMockLocation,
                Manifest.Permission.LocationHardware
            };
        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                // Register any platform specific implementations
            }
        }
    }
}
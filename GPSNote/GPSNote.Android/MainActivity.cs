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
using Android.Support.Annotation;
using GPSNote.Helpers;

namespace GPSNote.Droid
{
    
    [Activity(Label = nameof(GpsNote), Theme = "@style/MainTheme", Icon = "@mipmap/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(new[] {Android.Content.Intent.ActionView},
                  DataSchemes =new[] { Constants.LINK_PROTOCOL_HTTP, Constants.LINK_PROTOCOL_HTTPS },
                  DataHost = Constants.LINK_DOMEN,
                  DataPathPrefix = Constants.LINK_SEPARATOR,
                  AutoVerify =true,
                  Categories = new[] {Android.Content.Intent.ActionView, 
                                      Android.Content.Intent.CategoryBrowsable, 
                                      Android.Content.Intent.CategoryDefault})]

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
            global::Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
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
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.Internet) != Android.Content.PM.Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.AccessNetworkState) != Android.Content.PM.Permission.Granted ||
                    CheckSelfPermission(Manifest.Permission.ControlLocationUpdates) != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }

                else
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert("all good");
                    // Permissions already granted - display a message.
                }
            }
        }

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
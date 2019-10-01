using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using MDNSPublisher.Services;

namespace MDNSPublisher.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
      public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button startButton = FindViewById<Button>(Resource.Id.startButton);
            Button stopButton = FindViewById<Button>(Resource.Id.stopButton);
            startButton.Click += delegate { start(); };
            stopButton.Click += delegate { stop(); };
        }

        private void start()
        {
            var sP = new MDNSServicePublisher();
        }

        private void stop()
        {

        }
    }


}

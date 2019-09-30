using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Makaretu.Dns;
using Plugin.CurrentActivity;

namespace MDNSPublisher.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
        : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            App.Initialize();

            var mdns = new MulticastService();

            foreach (var a in MulticastService.GetIPAddresses())
            {
                System.Diagnostics.Debug.WriteLine($"IP address {a}");
            }

            mdns.QueryReceived += (s, e) =>
            {
                var names = e.Message.Questions
                    .Select(q => q.Name + " " + q.Type);
                System.Diagnostics.Debug.WriteLine($"got a query for {String.Join(", ", names)}");
            };
            mdns.AnswerReceived += (s, e) =>
            {
                var names = e.Message.Answers
                    .Select(q => q.Name + " " + q.Type)
                    .Distinct();
                System.Diagnostics.Debug.WriteLine($"got answer for {String.Join(", ", names)}");
            };
            mdns.NetworkInterfaceDiscovered += (s, e) =>
            {
                foreach (var nic in e.NetworkInterfaces)
                {
                    System.Diagnostics.Debug.WriteLine($"discovered NIC '{nic.Name}'");
                }
            };

            var sd = new ServiceDiscovery(mdns);
            sd.Advertise(new ServiceProfile("ipfs1", "_ipfs-discovery._udp", 5010));
            sd.Advertise(new ServiceProfile("x1", "_xservice._tcp", 5011));
            sd.Advertise(new ServiceProfile("x2", "_xservice._tcp", 666));
            var z1 = new ServiceProfile("z1", "_zservice._udp", 5012);
            z1.AddProperty("foo", "bar");
            sd.Advertise(z1);

            mdns.Start();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);


        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
           
        }

        public void OnActivityDestroyed(Activity activity)
        {

        }

        public void OnActivityPaused(Activity activity)
        {

        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {

        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {

        }
    }
}

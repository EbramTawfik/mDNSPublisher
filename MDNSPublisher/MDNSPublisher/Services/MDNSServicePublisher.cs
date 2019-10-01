using System;
using Makaretu.Dns;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MDNSPublisher.Services
{
    public class MDNSServicePublisher
    {
        public MDNSServicePublisher()
        {
            Task.Run(() => {
                var mdns = new MulticastService();
                mdns.UseIpv6 = false;
                foreach (var a in MulticastService.GetIPAddresses())
                {
                    Debug.WriteLine($"IP address {a}");
                }

                mdns.QueryReceived += (s, e) =>
                {
                    var names = e.Message.Questions
                        .Select(q => q.Name + " " + q.Type);
                    Debug.WriteLine($"got a query for {String.Join(", ", names)}");
                };
                mdns.AnswerReceived += (s, e) =>
                {
                    var names = e.Message.Answers
                        .Select(q => q.Name + " " + q.Type)
                        .Distinct();
                    Debug.WriteLine($"got answer for {String.Join(", ", names)}");
                };
                mdns.NetworkInterfaceDiscovered += (s, e) =>
                {
                    foreach (var nic in e.NetworkInterfaces)
                    {
                        Debug.WriteLine($"discovered NIC '{nic.Name}'");
                    }
                };

                var sd = new ServiceDiscovery(mdns);

                var s1 = new ServiceProfile("EbramMobile", "_airplay._tcp.", 7000);
                s1.AddProperty("deviceid", "00:05:a6:16:45:4b");
                s1.AddProperty("features", "0xA7FFFF7,0xE");
                s1.AddProperty("flags", "0x4");
                s1.AddProperty("model", "AppleTV5,3");
                s1.AddProperty("pi", "6b448552-85ce-4143-a896-e28d12e8a0ab");
                s1.AddProperty("pk", "F381DC574DEAF9C70B75297755BC7C7C35BB1D0DB500258F3AB46B5FE7C7355B");
                s1.AddProperty("srcvers", "220.68");
                s1.AddProperty("vv", "2");
                sd.Advertise(s1);
                mdns.Start();
            }
            );
         
        }
    }
}

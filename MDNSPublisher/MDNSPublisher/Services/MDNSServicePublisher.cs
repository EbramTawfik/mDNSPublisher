using System;
using Makaretu.Dns;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

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

                var address = new List<IPAddress>();
                address.Add( IPAddress.Parse("10.113.81.192"));

                var s1 = new ServiceProfile("JOSHMobile", "_airplay._tcp.", 7000,address);
                s1.AddProperty("deviceid", "00:05:A6:16:45:8F");
                s1.AddProperty("features", "0xA7FFFF7,0xE");
                s1.AddProperty("flags", "0x4");
                s1.AddProperty("model", "AppleTV5,3");
                s1.AddProperty("pi", "6b448552-85ce-4143-a896-e28d12e8a0ab");
                s1.AddProperty("pk", "F381DC574DEAF9C70B75297755BC7C7C35BB1D0DB500258F3AB46B5FE7C7355B");
                s1.AddProperty("srcvers", "220.68");
                s1.AddProperty("vv", "2");



                var s2 = new ServiceProfile("0005A616458F@JOSHMobile", "_raop._tcp.", 7000,address);
                s2.AddProperty("am", "AppleTV5,3");
                s2.AddProperty("ch", "2");
                s2.AddProperty("cn", "0,1,2,3");
                s2.AddProperty("da", "true");
                s2.AddProperty("ek", "1");
                s2.AddProperty("et", "0,3,5");
                s2.AddProperty("md", "0,1,2");
                s2.AddProperty("pw", "false");
                s2.AddProperty("sm", "false");
                s2.AddProperty("sr", "44100");
                s2.AddProperty("ss", "16");
                s2.AddProperty("sv", "false");
                s2.AddProperty("tp", "UDP");
                s2.AddProperty("txvers", "1");
                s2.AddProperty("vn", "65537");
                s2.AddProperty("vs", "220.68");
                s2.AddProperty("sf", "0x4");
                s2.AddProperty("ft", "0xA7FFFF7,0xE");
                s2.AddProperty("pk", "F381DC574DEAF9C70B75297755BC7C7C35BB1D0DB500258F3AB46B5FE7C7355B");
                s2.AddProperty("vv", "2");

                sd.Advertise(s1);
                sd.Advertise(s2);
                mdns.Start();
            }
            );
         
        }
    }
}

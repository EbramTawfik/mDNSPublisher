using System.Windows.Input;
using MDNSPublisher.Services;

namespace MDNSPublisher
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => {
                Plugin.Share.CrossShare.Current.OpenBrowser("https://xamarin.com/platform");
                MDNSServicePublisher m = new MDNSServicePublisher();
            });
        }

        public ICommand OpenWebCommand { get; }
    }
}

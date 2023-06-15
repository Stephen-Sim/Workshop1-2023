using RegisterMobile.Models;
using RegisterMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterMobile
{
    public partial class App : Application
    {
        public static Competitor Competitor { get; set; }
        public App()
        {
            InitializeComponent();

            Competitor = new Competitor();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

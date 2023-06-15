using Session5_Mobile.Models;
using Session5_Mobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5_Mobile
{
    public partial class App : Application
    {
        public static User User;
        public App()
        {
            InitializeComponent();

            User = new User();

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

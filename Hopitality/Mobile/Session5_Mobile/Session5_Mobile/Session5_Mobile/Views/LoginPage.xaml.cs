using Session5_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public Session5Service service { get; set; }
        public LoginPage()
        {
            InitializeComponent();

            service = new Session5Service();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var res = await service.login(uEntry.Text, pEntry.Text);

            if (res)
            {
                await DisplayAlert("Alert", "Successfully login.", "Ok");

                App.Current.MainPage = new NavigationPage(new ServicePage());
            }
            else
            {
                await DisplayAlert("Alert", "Invalid Login Credential.", "Ok");
            }
        }
    }
}
using RegisterMobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public GetService getService { get; set; }
        public LoginPage()
        {
            InitializeComponent();

            getService = new GetService();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameEditor.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
            {
                await DisplayAlert("Alert", "All the fields are requried.", "Ok");
                return;
            }

            var res = await getService.Login(UsernameEditor.Text, PasswordEntry.Text);

            if (res)
            {
                await DisplayAlert("Alert", "Successully Login", "Ok");

                App.Current.MainPage = new NavigationPage();
                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Alert", "User not found", "Ok");
            }
        }
    }
}
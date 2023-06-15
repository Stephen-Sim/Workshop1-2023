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
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            CartButton.Text = $"Cart ({App.User.AddOnServiceCount})";
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new ServicePage());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CartCheckoutPage());
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

        }
    }
}
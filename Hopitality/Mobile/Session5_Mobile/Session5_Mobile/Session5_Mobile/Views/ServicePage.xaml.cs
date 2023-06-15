using Session5_Mobile.Models;
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
    public partial class ServicePage : ContentPage
    {
        public Session5Service service { get; set; }
        public ServicePage()
        {
            InitializeComponent();

            service = new Session5Service();

            CartButton.Text = $"Cart ({App.User.AddOnServiceCount})";

            loadDataAsync();
        }

        private async void loadDataAsync()
        {
            fnSpan.Text = App.User.FullName;

            var result = await service.getServiceTypes();

            if (result != null)
            {
                stListView.ItemsSource = result;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CartCheckoutPage());
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AboutPage());
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            var obj = (ServiceType) (sender as Grid).BindingContext;
            await App.Current.MainPage.Navigation.PushAsync(new ServiceSelectionPage(obj));
        }
    }
}
using RegisterMobile.Models;
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
    public partial class MainPage : ContentPage
    {
        public Competitor competitor { get; set; }

        public GetService getService { get; set; }
        public MainPage()
        {
            InitializeComponent();

            getService = new GetService();
            competitor = App.Competitor;

            this.loadListView();

            this.BindingContext = this;
        }

        private async void loadListView()
        {
            var res = await getService.GetCompetitions(competitor.Id);

            if (res != null)
            {
                CompetitionListView.ItemsSource = res;
            }   
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Alert", "Are you sure to make change the status?", "yes", "no");

            if (res == true)
            {
                var com = (Competition)(sender as Grid).BindingContext;
                var result = await getService.ChangeStatus(com.Id);

                if (result)
                {
                    await DisplayAlert("Alert", "Status Changed!", "Ok");
                    this.loadListView();
                }
                else
                {
                    await DisplayAlert("Alert", "Something went wrong! Please try again!", "Ok");
                }
            }
        }
    }
}
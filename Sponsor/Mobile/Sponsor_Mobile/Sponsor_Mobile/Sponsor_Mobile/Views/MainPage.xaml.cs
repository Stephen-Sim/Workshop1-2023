using Sponsor_Mobile.Models;
using Sponsor_Mobile.Services;
using Sponsor_Mobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sponsor_Mobile
{
    public partial class MainPage : ContentPage
    {
        public List<Temp> CountryList { get; set; }
        public List<Temp> SkillList { get; set; }
        public MainPage()
        {
            InitializeComponent();
            getService = new GetService();

            TotalComLabel.Text = $"Total Competitor : 0";

            CountryList = new List<Temp>();
            CountryList.Add(new Temp { Id = 1, Name = "Philippines" });
            CountryList.Add(new Temp { Id = 2, Name = "Indonesia" });
            CountryList.Add(new Temp { Id = 3, Name = "Cambodia" });
            CountryList.Add(new Temp { Id = 4, Name = "Vietnam" });
            CountryList.Add(new Temp { Id = 5, Name = "Malaysia" });
            CountryList.Add(new Temp { Id = 6, Name = "Singapore" });
            CountryList.Add(new Temp { Id = 7, Name = "Philippines" });


            CountryPicker.ItemsSource = CountryList;
            CountryPicker.ItemDisplayBinding = new Binding("Name");

            SkillList = new List<Temp>();
            SkillList.Add(new Temp { Id = 1, Name = "Electrical Installations" });
            SkillList.Add(new Temp { Id = 2, Name = "Hairdressing" });
            SkillList.Add(new Temp { Id = 3, Name = "IT Network Systems Administration" });
            SkillList.Add(new Temp { Id = 4, Name = "Electronics" });
            SkillList.Add(new Temp { Id = 5, Name = "Mechatronics" });
            SkillList.Add(new Temp { Id = 6, Name = "Heavy Vehicle Technology" });

            SkillPicker.ItemsSource = SkillList;
            SkillPicker.ItemDisplayBinding = new Binding("Name");

            _ = this.loadCompetitors();
        }

        public GetService getService { get; set; }

        public async Task loadCompetitors(long? cid = null, long? sid = null)
        {
            var result = await getService.GetCompetitors(cid, sid);
            
            if (result != null)
            {
                CompetitorListView.ItemsSource = result;
                TotalComLabel.Text = $"Total Competitor : {result.Count}";
            }
        }

        private void CountryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cid = CountryPicker.SelectedIndex != -1 ?  ((Temp)CountryPicker.SelectedItem)?.Id : null;
            var sid = SkillPicker.SelectedIndex != -1 ? ((Temp)SkillPicker.SelectedItem)?.Id : null;

            _ = this.loadCompetitors(cid, sid);
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            var com = (Competitor)(sender as ImageButton).CommandParameter;

            if (com.Color == "Green")
            {
                await DisplayAlert("Alert", "The competitor has already fully sponsor!!", "ok");
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new PaymentPage(com));
        }
    }
}

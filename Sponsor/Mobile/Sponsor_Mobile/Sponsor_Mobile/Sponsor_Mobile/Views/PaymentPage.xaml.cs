using Sponsor_Mobile.Models;
using Sponsor_Mobile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sponsor_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public GetService getService { get; set; }

        private Competitor com;

        public Competitor Competitor
        {
            get { return com; }
            set { com = value; OnPropertyChanged(); }
        }

        public PaymentPage(Competitor Competitor)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.Competitor = Competitor;

            MaxLabel.Text = $"Maximum Amount: SGD {this.Competitor.RequiredAmount}";

            getService = new GetService();
            _ = this.loadCaptcha();
            _ = loadCurrencies();
        }

        private string captchatext = null;
        private bool isCaptchaVerified = false;

        private decimal MaxAmount = 0.00m;

        private async Task loadCaptcha()
        {
            var res = await getService.GetCaptcha();

            if (res != null)
            {
                Stream stream = new MemoryStream(res.CaptchaByteArr);
                CaptchaImage.Source = ImageSource.FromStream(() => stream);
                captchatext = res.CaptcapText;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (captchatext == CaptchaEditor.Text)
            {
                CaptchaStackLayout.IsVisible = false;
                HumanLabel.IsVisible = true;
                isCaptchaVerified = true;
            }
            else
            {
                CaptchaEditor.Text = string.Empty;
                await DisplayAlert("Alert", "Wrong Captcha!", "Ok");
                _ = this.loadCaptcha();
            }
        }

        private async Task loadCurrencies()
        {
            var res = await getService.GetCurrencies();

            if (res != null)
            {
                CurrencyPicker.ItemsSource = res;
                CurrencyPicker.ItemDisplayBinding = new Binding("Name");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SponsornameEditor.Text) || string.IsNullOrEmpty(DescEditor.Text) || decimal.Parse(AmountEditor.Text) <= 0.00m
                || string.IsNullOrEmpty(CardNumberEditor.Text) || string.IsNullOrEmpty(CVVEditor.Text) || CurrencyPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Alert", "All the feilds are required!", "Ok");
                return;
            }

            if (CardNumberEditor.Text.Length < 16 && CVVEditor.Text.Length < 3)
            {
                await DisplayAlert("Alert", "Invalid Card Number Length or CVV Length!", "Ok");
                return;
            }

            if (decimal.Parse(AmountEditor.Text) > MaxAmount)
            {
                await DisplayAlert("Alert", "Sponsor Amount exceeds the maximum amount!", "Ok");
                return;
            }

            if (!isCaptchaVerified)
            {
                await DisplayAlert("Alert", "Captcha is not verified!", "Ok");
                return;
            }

            if (!TermCheckbox.IsChecked)
            {
                await DisplayAlert("Alert", "Payment terms is not agree yet!", "Ok");
                return;
            }

            var card = new Card
            {
                CardNo = CardNumberEditor.Text,
                CVV = CVVEditor.Text,
                Amount = (decimal.Parse(AmountEditor.Text) * ((Currency)CurrencyPicker.SelectedItem).Rate)
            };

            var res = await getService.ValidateCard(card);

            if (res != "success")
            {
                await DisplayAlert("Alert", res, "Ok");

                return;
            }

            var sponsor = new Sponsorship
            {
                Amount = decimal.Parse(AmountEditor.Text),
                CompetitorId = this.Competitor.Id,
                DateTime = DateTime.Now,
                Description = DescEditor.Text,
                SponsorName = SponsornameEditor.Text,
                CurrencyId = ((Currency)CurrencyPicker.SelectedItem).Id,
            };

            var res1 = await getService.storeSponsor(sponsor);

            if (res1 == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Alert", "Payment Success!", "Ok");

                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await DisplayAlert("Alert", "Payment Failed, Please Try Again!", "Ok");
            }
        }

        private void CurrencyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCurrency = (Currency)CurrencyPicker.SelectedItem;
            AmountEditor.IsEnabled = true;
            MaxLabel.Text = $"Maximum Amount: {selectedCurrency.Name} {((decimal.Parse(this.Competitor.RequiredAmount) / selectedCurrency.Rate).ToString("0.00"))}";
            MaxAmount = decimal.Parse(this.Competitor.RequiredAmount) / selectedCurrency.Rate;
        }
    }
}
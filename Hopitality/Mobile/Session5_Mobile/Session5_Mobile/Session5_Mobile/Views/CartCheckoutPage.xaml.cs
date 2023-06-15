using Session5_Mobile.Models;
using Session5_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session5_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartCheckoutPage : ContentPage
    {
        public Coupon coupon { get; set; }

        private int count = 0;
        public decimal price { get; set; }
        public CartCheckoutPage()
        {
            InitializeComponent();

            service = new Session5Service();

            price = 0.0m;

            loadListView();
        }

        public Session5Service service { get; set; }

        private async void loadListView()
        {
            CartButton.Text = $"Cart ({App.User.AddOnServiceCount})";
            var res = await service.getAddonServiceDetails(App.User.AddOnServiceId);

            if (res != null)
            {
                CartListView.ItemsSource = res;

                price = res.Sum(x => x.Price);
                count = res.Count;

                PayAmountLabel.Text = $"Total Amount Payable ({count} items): $ {price.ToString("0.00")}";
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new ServicePage());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AboutPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Alert", "Are you sure to delete this booking?", "yes", "no");

            if (result)
            {
                var id = (long)(sender as Button).CommandParameter;

                var res = await service.delAddonServiceDetail(id);

                if (res)
                {
                    await DisplayAlert("Alert", "Booking deleted", "ok");
                    CartButton.Text = $"Cart ({App.User.AddOnServiceCount})";
                    loadListView();
                }
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var res = await service.checkCoupon(CouponEntry.Text);

            if (res != null)
            {
                coupon = res;
                IsSucceedAppliedLabel.Text = "Coupon Successully Applied!";

                decimal discountPrice = price * (coupon.DiscountPercent / 100);

                if (discountPrice > coupon.MaximimDiscountAmount)
                {
                    discountPrice = coupon.MaximimDiscountAmount;
                }

                PayAmountLabel.Text = $"Total Amount Payable ({count} items): $ {(price - discountPrice).ToString("0.00")}";
            }
            else
            {
                IsSucceedAppliedLabel.Text = "Invalid Coupon";
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (count == 0)
            {
                await DisplayAlert("Alert", "No service to pay", "Ok");
                return;
            }

            if (coupon == null)
            {
                await DisplayAlert("Alert", "Please insert coupon.", "Ok");
                return;
            }

            var result = await DisplayAlert("Alert", "Are you sure to Proceed payment?", "yes", "no");

            if (result)
            {
                var res = await service.proceedPayment(App.User.AddOnServiceId, coupon.ID);

                if (res)
                {
                    await DisplayAlert("Alert", "Payment Success.", "Ok");
                    CouponEntry.Text = string.Empty;
                    loadListView();
                }
            }
        }
    }
}
using FoodApp.Models;
using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceOrderPage : ContentPage
    {
        private double _totalPrice;
        public PlaceOrderPage(double totalPrice)
        {
            InitializeComponent();
            _totalPrice = totalPrice;
        }

        private async void BtnPlaceOrder_Clicked(object sender, EventArgs e)
        {
            var order = new Order();
            order.fullName = EntName.Text;
            order.address = EntAddress.Text;
            order.phone = EntPhone.Text;
            order.userId = Preferences.Get("userID", 0);
            order.orderTotal = _totalPrice;

            var apiService = new ApiService();
            var response = await apiService.PlaceOrder(order);
            if (response != null)
            {
                await DisplayAlert("", "Your order id is " + response.orderId, "Alright");
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }

        }
    }
}
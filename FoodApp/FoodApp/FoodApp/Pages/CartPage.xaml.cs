using FoodApp.Models;
using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<ShoppingCartItem> CartItems;
        public CartPage()
        {
            InitializeComponent();
            CartItems = new ObservableCollection<ShoppingCartItem>();
            GetShoppingCartItems();
        }

        private async void GetShoppingCartItems()
        {
            
            var apiService = new ApiService();
            var shoppingCartItems = await apiService.GetShoppingCartItems(Preferences.Get("userID", 0));
            double total = new double();
            /*Product product = null;*/
            foreach (var cartItem in shoppingCartItems)
            {
                CartItems.Add(cartItem);
                total += cartItem.TotalAmount;
                /*product = await apiService.GetProductByID(cartItem.ProductId);*/
                
            }
            LvShoppingCart.ItemsSource = CartItems;
            LblTotalPrice.Text = total.ToString();
        }

        private async void TapClearCart_Tapped(object sender, EventArgs e)
        {
            var apiService = new ApiService();
            var shoppingCartItems = await apiService.GetShoppingCartItems(Preferences.Get("userID", 0));

            var response = await apiService.ClearShoppingCart(Preferences.Get("userID", 0));
            if (response)
            {
                await DisplayAlert("", "Your cart has been cleared", "Alright");
                LvShoppingCart.ItemsSource = null;
                LblTotalPrice.Text = "0";
            }
            else
            {
                await DisplayAlert("", "Something went wrong", "Cancel");
            }
        }

        private void BtnProceed_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PlaceOrderPage(Convert.ToDouble(LblTotalPrice.Text)));
        }
    }
}
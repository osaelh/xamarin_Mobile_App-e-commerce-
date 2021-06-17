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
    public partial class ProductDetailPage : ContentPage
    {

        private int _productId;
        public ProductDetailPage(int productId)
        {
            InitializeComponent();
            GetProductDetail(productId);
            _productId = productId;
        }

        private async void GetProductDetail(int productId)
        {
            var apiService = new ApiService();
            var product = await apiService.GetProductByID(productId);
            LblName.Text = product.name;
            LblPrice.Text = product.price.ToString();
            LblDetail.Text = product.detail;
            ImgProduct.Source = product.FullImageUrl; 
        }

        private void TapIncrement_Tapped(object sender, EventArgs e)
        {
            var i = Convert.ToInt16( LblQty.Text);
            i++;
            LblQty.Text = i.ToString();
            LblTotalPrice.Text = (Convert.ToInt16(LblQty.Text) * Convert.ToInt16(LblPrice.Text)).ToString();
        }

        private void TapDecrement_Tapped(object sender, EventArgs e)
        {
            var i = Convert.ToInt16(LblQty.Text);
            i--;
            if (i < 1)
            {
                return;
            }
            LblQty.Text = i.ToString();
            LblTotalPrice.Text = (Convert.ToInt16(LblQty.Text) * Convert.ToInt16(LblPrice.Text)).ToString();
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void BtnAddToCart_Clicked(object sender, EventArgs e)
        {
            var addToCart = new ShoppingCartItem();
            addToCart.Qty = Convert.ToInt16( LblQty.Text);
            addToCart.Price = Convert.ToInt16(LblPrice.Text);
            addToCart.TotalAmount= Convert.ToInt16(LblTotalPrice.Text);
            addToCart.ProductId = _productId;
            addToCart.ProductName = LblName.Text;
            addToCart.CustomerId = Preferences.Get("userID", 0);

            var apiService = new ApiService();
            var response = await apiService.AddItemToCart(addToCart);
            if (response)
            {
                await DisplayAlert("", "Your item has been added to the cart", "Alright");
            }
            else
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }
    }
}
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
    public partial class HomePage : ContentPage
    {

        public ObservableCollection<PopularProduct> ProductsCollection { get; set; }
        public ObservableCollection<Category> CategoryCollection { get; set; }
        public HomePage()
        {
            InitializeComponent();
            ProductsCollection = new ObservableCollection<PopularProduct>();
            CategoryCollection = new ObservableCollection<Category>();
            GetCategories();
            GetPopularProducts();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
        }

        private async void GetCategories()
        {
            var apiService = new ApiService();
            var categories = await apiService.GetCategories();

            foreach (var cat in categories)
            {
                CategoryCollection.Add(cat);
            }
            CvCategories.ItemsSource = CategoryCollection;
        }

        private async void  GetPopularProducts()
        {
            var apiService = new ApiService();
            var products = await apiService.GetPopularProducts();
            foreach (var product in products)
            {
                ProductsCollection.Add(product);
                
            }

            CvProducts.ItemsSource = ProductsCollection;
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private async void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
          CloseHamburgerMenu();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var apiService = new ApiService();
            var response = await apiService.GetTotalCartItem(Preferences.Get("userID", 0));
            LblTotalItems.Text = response.totalItems.ToString();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CloseHamburgerMenu();
        }

        private async void CloseHamburgerMenu()
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Category;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductsListPage(currentSelection.id, currentSelection.name));
            ((CollectionView)sender).SelectedItem = null;

        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as PopularProduct;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currentSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void TapCartIcon_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }

        private void TapOrders_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new OrdersPage());
        }
    }
}
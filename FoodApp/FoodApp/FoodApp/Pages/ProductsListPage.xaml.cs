using FoodApp.Models;
using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsListPage : ContentPage
    {
        public ObservableCollection<ProductByCategory> ProductByCategorieCollection;
        public ProductsListPage(int categoryId, string categoryName)
        {
            InitializeComponent();
            LblCategoryName.Text = categoryName;
            GetProducts(categoryId);
            ProductByCategorieCollection = new ObservableCollection<ProductByCategory>();
        }

        private async void GetProducts(int categoryId)
        {
            var apiService = new ApiService();
            var products = await apiService.GetProductsByCategory(categoryId);
            foreach (var product in products)
            {
                ProductByCategorieCollection.Add(product);
            }
            CvProducts.ItemsSource = ProductByCategorieCollection;
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();

        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as ProductByCategory;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currentSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
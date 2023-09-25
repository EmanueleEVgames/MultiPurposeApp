using FoodPreferences.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FoodPreferences.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
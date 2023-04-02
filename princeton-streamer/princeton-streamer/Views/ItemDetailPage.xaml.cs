using princeton_streamer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace princeton_streamer.Views
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
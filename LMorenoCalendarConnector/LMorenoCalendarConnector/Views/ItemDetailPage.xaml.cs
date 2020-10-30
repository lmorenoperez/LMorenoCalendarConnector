using System.ComponentModel;
using Xamarin.Forms;
using LMorenoCalendarConnector.ViewModels;

namespace LMorenoCalendarConnector.Views
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
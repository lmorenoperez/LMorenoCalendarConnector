using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LMorenoCalendarConnector.Services;
using LMorenoCalendarConnector.Views;
using LMorenoCalendarConnector.Abstractions;

namespace LMorenoCalendarConnector
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<ICalendarConnector>();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

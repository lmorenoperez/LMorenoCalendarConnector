using LMorenoCalendarConnector.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LMorenoCalendarConnector.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public Command LearnMoreClickCommand { get; }

        public AboutViewModel()
        {
            Title = "About";
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
            LearnMoreClickCommand = new Command(OnLearnMoreClicked);
        }

        public ICommand OpenWebCommand { get; }

        private async void OnLearnMoreClicked(object obj)
        {
            //var _permissionsUtil = CrossPermissions.Current;// DependencyService.Get<IPermissions>();
            //await _permissionsUtil.RequestPermissionsAsync(new Permission[] { Permission.Calendar, Permission.Contacts, Permission.Storage });

            //
            DateTime dt1 = DateTime.Now.AddMinutes(1);
            DependencyService.Get<ICalendarConnector>().AddAppointment(DateTime.Now.AddHours(5), DateTime.Now.AddHours(5).AddMinutes(30), "Testing Carlendar 5 hours", "Miami", "Miami Boat Show", false,AppointmentReminder.five, AppointmentStatus.busy,"",2,"UTC");


            //await Application.Current.MainPage.DisplayAlert("AA", "BB", "Ok");
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
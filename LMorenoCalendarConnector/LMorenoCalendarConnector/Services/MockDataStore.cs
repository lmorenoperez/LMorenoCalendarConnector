using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMorenoCalendarConnector.Abstractions;
using LMorenoCalendarConnector.Models;
using Xamarin.Forms;

namespace LMorenoCalendarConnector.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            DependencyService.Get<ICalendarConnector>().AddAppointment(item.AppointmentDateTime, item.AppointmentDateTime.AddMinutes(30), item.Text, "Store Location", item.Description, false, AppointmentReminder.five, AppointmentStatus.busy, "DAILY", 3,"UTC");

            await Application.Current.MainPage.DisplayAlert("Notice", "Reminder added successfully", "Ok");

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            var DeviceCalendarItems = DependencyService.Get<ICalendarConnector>().GetAppointments("1");
            
            return await Task.FromResult(DeviceCalendarItems);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventKit;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using LMorenoCalendarConnector.iOS;
using LMorenoCalendarConnector.Abstractions;
using LMorenoCalendarConnector.Models;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarConnector))]
namespace LMorenoCalendarConnector.iOS
{
    
    public class CalendarConnector : ICalendarConnector
    {
        public static void Init() { eventStore = new EKEventStore(); }

        public EKEventStore EventStore
        {
            get { return eventStore; }
        }
        protected static EKEventStore eventStore;


        public Task<bool> RequestAccessAsync()
        {
            var taskSource = new TaskCompletionSource<bool>();

            this.EventStore.RequestAccess(EKEntityType.Event,
                    (bool granted, NSError e) =>
                    {
                        if (!granted)
                            taskSource.SetException(new Exception("User Denied Access to Calendar Data"));
                        else
                            taskSource.SetResult(true);
                    });

            return taskSource.Task;
        }


        public async void AddAppointment(DateTime startTime, DateTime endTime, String subject, String location, String details, Boolean isAllDay, AppointmentReminder reminder, AppointmentStatus status, string sFreq, int iFreqCount, string sEventTimezone)
        {
            var granted = await RequestAccessAsync();

            if (granted)
            {
                EKEvent newEvent = EKEvent.FromStore(this.EventStore);
                newEvent.StartDate = (NSDate)DateTime.SpecifyKind(startTime, DateTimeKind.Utc);
                newEvent.EndDate = (NSDate)DateTime.SpecifyKind(endTime, DateTimeKind.Utc);
                newEvent.Title = subject;
                newEvent.Location = location;
                newEvent.Notes = details;
                newEvent.AllDay = isAllDay;
                newEvent.AddAlarm(ConvertReminder(reminder, startTime));
                newEvent.Availability = ConvertAppointmentStatus(status);

                NSError error;
                this.EventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out error);

                Console.WriteLine("Event Saved, ID: " + newEvent.CalendarItemIdentifier);
            }
        }

        //

        public List<Item> GetAppointments(string CalendarId)
        {
            List<Item> CalendarItems = new List<Item>();

            CalendarItems.Add(new Item { Id = "1", Text = "1", Description = "1", AppointmentDateTime = DateTime.Now });

            return CalendarItems;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reminder"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>


        private EKAlarm ConvertReminder(AppointmentReminder reminder, DateTime startTime)
        {
            switch (reminder)
            {
                case AppointmentReminder.none:
                    return EKAlarm.FromDate((NSDate)startTime); ///todo should this be null?
                case AppointmentReminder.five:
                    return EKAlarm.FromDate((NSDate)startTime.AddMinutes(-5));
                case AppointmentReminder.fifteen:
                    return EKAlarm.FromDate((NSDate)startTime.AddMinutes(-15));
                case AppointmentReminder.thirty:
                    return EKAlarm.FromDate((NSDate)startTime.AddMinutes(-30));
            }
            return EKAlarm.FromDate((NSDate)startTime);
        }


        private EKEventAvailability ConvertAppointmentStatus(AppointmentStatus status)
        {
            switch (status)
            {
                case AppointmentStatus.busy:
                    return EKEventAvailability.Busy;
                case AppointmentStatus.free:
                    return EKEventAvailability.Free;
                case AppointmentStatus.tentative:
                    return EKEventAvailability.Tentative;
                case AppointmentStatus.outofoffice:
                    return EKEventAvailability.Unavailable;
            }
            return EKEventAvailability.NotSupported;
        }


    }
}
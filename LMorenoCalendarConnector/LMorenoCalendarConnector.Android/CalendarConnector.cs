using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LMorenoCalendarConnector.Abstractions;
using LMorenoCalendarConnector.Droid;
using Java.Util;
using LMorenoCalendarConnector.Models;
using Android.Database;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarConnector))]
namespace LMorenoCalendarConnector.Droid
{
    public class CalendarConnector : ICalendarConnector
    {

        private static Activity Activity;
        
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        /// 
        public static void Init(Activity activity) { 
            CalendarConnector.Activity = activity;
            
        }


        public void AddAppointment(DateTime startTime, DateTime endTime, String subject, String location, String details, Boolean isAllDay, AppointmentReminder reminder, AppointmentStatus status, string sFreq , int iFreqCount, string sEventTimezone)
        {

            ContentValues eventValues = new ContentValues();
            eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, 1);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, subject);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, details);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(startTime));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(endTime));

            // GitHub issue #9 : Event start and end times need timezone support.
            // https://github.com/xamarin/monodroid-samples/issues/9
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, sEventTimezone);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, sEventTimezone);
            if (!String.IsNullOrEmpty(sFreq))
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Rrule, String.Format("FREQ={0};COUNT={1}", sFreq, iFreqCount));//  "FREQ=DAILY;COUNT=2"); ;

            //values.put(CalendarContract.Events.RRULE, "FREQ=DAILY;COUNT=4");
            var uri = CalendarConnector.Activity.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
            long eventID = long.Parse(uri.LastPathSegment);
            ContentValues remindervalues = new ContentValues();
            remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Minutes, ConvertReminder(reminder));
            remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.EventId, eventID);
            remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Method, (int)RemindersMethod.Alert);
            var reminderURI = CalendarConnector.Activity.ContentResolver.Insert(CalendarContract.Reminders.ContentUri, remindervalues);

            Console.WriteLine("Uri for new event: {0}", uri);


        }

        public List<Item> GetAppointments(string CalendarId)
        {
            List<Item> CalendarItems = new List<Item>();
            var eventsUri = CalendarContract.Events.ContentUri;

            string[] eventsProjection = {
                CalendarContract.Events.InterfaceConsts.Id,
                CalendarContract.Events.InterfaceConsts.Title,
                CalendarContract.Events.InterfaceConsts.Dtstart
             };

            var loader = new CursorLoader(CalendarConnector.Activity, eventsUri, eventsProjection,
                               String.Format("calendar_id={0}", CalendarId), null, "dtstart ASC");
            var cursor = (ICursor)loader.LoadInBackground();

            string[] sourceColumns = {
                CalendarContract.Events.InterfaceConsts.Title,
                CalendarContract.Events.InterfaceConsts.Dtstart
            };

           

            while (cursor.MoveToNext())
            {

                string CalendarEvent_Id = cursor.GetString(0);
                string EventTitle = cursor.GetString(1);
                string EventInMillsec = cursor.GetString(2);
                double dlEventMil = 0;
                if (double.TryParse(EventInMillsec, out dlEventMil))
                {
                    var EventDt = (new DateTime(1970, 1, 1)).AddMilliseconds(dlEventMil);
                    string EventDtString = EventDt.ToLongDateString() + " " + EventDt.ToLongTimeString();
                    if (EventDt >= DateTime.Now)
                        CalendarItems.Add(new Item { Id = cursor.GetString(0), Text = cursor.GetString(1), Description = EventDtString, AppointmentDateTime = EventDt });
                }
            }

            return CalendarItems;
        }


        long GetDateTimeMS(int yr, int month, int day, int hr, int min)
        {
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Java.Util.CalendarField.DayOfMonth, day);
            c.Set(Java.Util.CalendarField.HourOfDay, hr);
            c.Set(Java.Util.CalendarField.Minute, min);
            c.Set(Java.Util.CalendarField.Month, month);
            c.Set(Java.Util.CalendarField.Year, yr);

            return c.TimeInMillis;
        }
        long GetDateTimeMS(DateTime dt)
        {
            int yr = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            int hr = dt.Hour;
            int min = dt.Minute;
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Java.Util.CalendarField.DayOfMonth, day);
            c.Set(Java.Util.CalendarField.HourOfDay, hr);
            c.Set(Java.Util.CalendarField.Minute, min);
            c.Set(Java.Util.CalendarField.Month, 9);
            c.Set(Java.Util.CalendarField.Year, yr);

            return c.TimeInMillis;
        }

        private int ConvertReminder(AppointmentReminder reminder)
        {
            switch (reminder)
            {
                case AppointmentReminder.none:
                    return 0; ///todo should this be null?
                case AppointmentReminder.five:
                    return 5;
                case AppointmentReminder.fifteen:
                    return 15;
                case AppointmentReminder.thirty:
                    return 30;
            }
            return 0;
        }


        private string ConvertAppointmentStatus(AppointmentStatus status)
        {
            switch (status)
            {
                case AppointmentStatus.busy:
                    return "Busy";
                case AppointmentStatus.free:
                    return "Free";
                case AppointmentStatus.tentative:
                    return "Tentative";
                case AppointmentStatus.outofoffice:
                    return "Unavailable";
            }
            return "";
        }

       
    }
}
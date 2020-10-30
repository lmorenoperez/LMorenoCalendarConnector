using LMorenoCalendarConnector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMorenoCalendarConnector.Abstractions
{
    public interface ICalendarConnector
    {
        void AddAppointment(DateTime startTime, DateTime endTime, String subject, String location, String details, Boolean isAllDay, AppointmentReminder reminder, AppointmentStatus status, string sFreq, int iFreqCount, string sEventTimezone);

        List<Item> GetAppointments(string CalendarId);
    }
}

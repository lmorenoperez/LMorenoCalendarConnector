using System;
using System.Collections.Generic;
using System.Text;

namespace LMorenoCalendarConnector.Abstractions
{
    public enum AppointmentReminder
    {
        none,
        five,
        fifteen,
        thirty
    }

    public enum AppointmentStatus
    {
        busy,
        free,
        tentative,
        outofoffice
    }
}

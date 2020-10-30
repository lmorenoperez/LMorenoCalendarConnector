using System;

namespace LMorenoCalendarConnector.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public DateTime AppointmentDateTime { get; set; }
    }
}
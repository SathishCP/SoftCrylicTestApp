using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftCrylicTestApp.Models
{
    public class Events
    {
        public List<EventManager> EventManager { get; set; } = new List<EventManager>();
    }

    public struct EventManager
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventDate { get; set; }
        public string EventMode { get; set; }
        public string EventVenue { get; set; }
        public string EventSite { get; set; }
        public string EventLink { get; set; }
        public int? SpeakerId { get; set; }
    }
}
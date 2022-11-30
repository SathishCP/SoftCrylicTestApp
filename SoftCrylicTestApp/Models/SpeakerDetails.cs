using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftCrylicTestApp.Models
{
    public class Speakers
    {
        public List<SpeakerDetail> SpeakerDetails { get; set; } = new List<SpeakerDetail>();
    }

    public struct SpeakerDetail
    {
        public short SpeakerSession { get; set; }
        public string SpeakerWeb { get; set; }
        public string SpeakerBio { get; set; }
        public string SpeakerName { get; set; }
        public int? SpeakerId { get; set; }
    }
}
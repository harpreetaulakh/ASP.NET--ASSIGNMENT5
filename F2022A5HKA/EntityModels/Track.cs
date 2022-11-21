using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F2022A5HKA.EntityModels
{
    public class Track
    {
        public string Clerk { get; set; } // holds the username (e.g. amanda@example.com) of the authenticated user who is in the process of adding a new Track object

        public string Composers { get; set; } //holds the names of the track's composers. user will simply type comma separators between the names of multiple composers so no hard-coding is required.

        public string Genre { get; set; } //holds a genre string/value

        public int TrackId { get; set; }

        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
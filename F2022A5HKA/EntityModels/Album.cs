using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F2022A5HKA.EntityModels
{
    public class Album
    {
        public Album()
        {
            Tracks = new HashSet<Track>();
            ReleaseDate = DateTime.Now.AddYears(-15);
        }

        public string Coordinator { get; set; } //  holds the username (e.g. amanda@example.com) of the authenticated user who is in the process of adding a new Album object

        public string Genre { get; set; } // holds a genre string/value. creating a new Album object, the available genres will be shown in an item-selection element

        public int AlbumId { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string UrlAlbum { get; set; } //hold the a URL to an image of the album

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }
}
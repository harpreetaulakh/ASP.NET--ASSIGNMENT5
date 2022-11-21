using F2022A5HKA.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace F2022A5HKA.Models
{
    public class AlbumAddViewModel
    {
        public AlbumAddViewModel()
        {
            TrackIds = new List<int>();
            ArtistIds = new List<int>();
            Artists = new List<Artist>();
            Tracks = new List<Track>();
            ReleaseDate = DateTime.Now;
        }

        [StringLength(40)]
        public string Coordinator { get; set; } //  holds the username (e.g. amanda@example.com) of the authenticated user who is in the process of adding a new Album object

        [StringLength(30), Required]
        public string Genre { get; set; } // holds a genre string/value. creating a new Album object, the available genres will be shown in an item-selection element

        [StringLength(40), Required]
        public string Name { get; set; }

        [Display(Name = "Release Date"), DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Album photo URL"), Required]
        public string UrlAlbum { get; set; } //hold the a URL to an image of the album

        public int ArtistId { get; set; }

        public string ArtistName { get; set; }

        public IEnumerable<int> ArtistIds { get; set; }

        public IEnumerable<Artist> Artists { get; set; }

        public IEnumerable<int> TrackIds { get; set; }

        public IEnumerable<Track> Tracks { get; set; }

        [Display(Name = "All Artists")]
        public MultiSelectList ArtistList { get; set; }

        [Display(Name = "All Tracks")]
        public MultiSelectList TrackList { get; set; }


        [Display(Name = "Primary genre")]
        public SelectList AlbumGenreList { get; set; }

    }
}
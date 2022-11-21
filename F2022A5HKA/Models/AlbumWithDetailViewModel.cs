using F2022A5HKA.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F2022A5HKA.Models
{
    public class AlbumWithDetailViewModel : AlbumBaseViewModel
    {

        public AlbumWithDetailViewModel()
        {
            Artists = new List<Artist>();
            Tracks = new List<Track>();
            ArtistNames = new List<string>();
            ReleaseDate = DateTime.Now;
        }

        public IEnumerable<string> ArtistNames { get; set; }

        public IEnumerable<Artist> Artists { get; set; }

        public IEnumerable<Track> Tracks { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Album depiction")]
        public string Depiction { get; set; }
    }
}
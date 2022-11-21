using F2022A5HKA.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F2022A5HKA.Models
{
    public class ArtistWithDetailViewModel : ArtistBaseViewModel
    {

        public ArtistWithDetailViewModel()
        {
            Albums = new List<Album>();
            AlbumNames = new List<string>();
            BirthOrStartDate = DateTime.Now;
        }

        public IEnumerable<Album> Albums { get; set; }

        public string ArtistName { get; set; }

        public IEnumerable<string> AlbumNames { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Artist portrayal")]
        public string Portrayal { get; set; }
    }
}
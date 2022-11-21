using F2022A5HKA.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F2022A5HKA.Models
{
    public class TrackWithDetailViewModel : TrackBaseViewModel
    {
        public TrackWithDetailViewModel()
        {
            Albums = new List<Album>();
            AlbumNames = new List<string>();
        }

        [Display(Name = "Albums with this track")]
        public IEnumerable<string> AlbumNames { get; set; }

        public int AlbumId { get; set; }

        public string AlbumName { get; set; }

        public IEnumerable<Album> Albums { get; set; }
    }
}
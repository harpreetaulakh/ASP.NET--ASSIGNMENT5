using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F2022A5HKA.EntityModels
{
    public class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
            BirthOrStartDate = DateTime.Now.AddYears(-15);
        }

        public int ArtistId { get; set; }

        public string BirthName { get; set; } //only used for a person who uses a stage name

        public DateTime BirthOrStartDate { get; set; } //birth date, for others it is the date that the artist started working in the music business.

       public string Executive { get; set; } //holds the username (e.g. amanda@example.com)

        public string Genre { get; set; } //This is used for when user create a new artist object, the available genres will be shown in an item-selection element.

        [StringLength(120)]
        public string Name { get; set; } //artist's name or stage/performer name

        public string UrlArtist { get; set; } //holds URL to photo of the artist

        public ICollection<Album> Albums { get; set; }
    }
}
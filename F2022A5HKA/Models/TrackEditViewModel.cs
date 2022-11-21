using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F2022A5HKA.Models
{
    public class TrackEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public HttpPostedFileBase AudioUpload { get; set; }

    }
}
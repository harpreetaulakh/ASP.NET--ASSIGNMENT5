using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F2022A5HKA.Models
{
    public class TrackAddFormViewModel : TrackAddViewModel
    {
        [Required, Display(Name = "Track audio"), DataType(DataType.Upload)]
        public string AudioUpload { get; set; }
    }
}
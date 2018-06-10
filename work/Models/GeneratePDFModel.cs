using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rotativa;

namespace work.Models
{
    public class GeneratePDFModel
    {
        public string PDFContent { get; set; }
        public string PDFLogo { get; set; }
        public Picture Picture { get; set; }
        public ApplicationUser Attendee { get; set; }

        public int PictureId { get; set; }

        public string AttendeeId { get; set; }
    }
}
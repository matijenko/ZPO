
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using work.Controllers;
using work.Models;

namespace work.ViewModels
{
    public class PictureFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }

        [Required]
        public byte Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public string Heading { get; set; }
    }
}
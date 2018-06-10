
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
    public class CategoryViewModel
    {
        public string Heading { get; set; }

        public List<Picture> Pictures { get; set; }
    }
}
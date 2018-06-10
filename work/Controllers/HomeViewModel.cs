using System.Collections.Generic;
using work.Models;

namespace work.Controllers
{
    public class HomeViewModel
    {
        public IEnumerable<Picture> UpcomingPictures { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
    }
}
using System.Collections;
using System.Collections.Generic;
using work.Models;

namespace work.ViewModels
{
    public class HomeViewModel : IEnumerable
    {
        public IEnumerable<Picture> UpcomingPictures { get; set; }
        public bool ShowActions { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
using System;

namespace work.Controllers.Api
{
    public class PictureDto
    {
        public int Id { get; set; }
        public bool IsCanceled { get; set; }
        public UserDto Artist { get; set; }
        public DateTime DateTime { get; set; }
        public string Venue { get; set; }
        public CategoryDto Category { get; set; }
    }
}
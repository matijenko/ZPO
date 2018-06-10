using System;
using work.Models;

namespace work.Controllers.Api
{

    public class NotificationDto
    {
        public DateTime DateTime { get; set; }
        public NotificationType Type { get; set; }
        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }
        public PictureDto Picture { get; set; }
        public UserDto Attender { get; set; }
    }
}
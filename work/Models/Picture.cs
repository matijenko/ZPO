using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace work.Models
{
    public class Picture
    {
        public int Id { get; set; }


        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }

        public string ContentType { get; set; }

        public Byte[] ImageContent { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        
        public Category Category { get; set; }

        [NotMapped]
        public bool IsFollowed { get; set; }

        [Required]
        public byte CategoryId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Picture()
        {
            Attendances = new Collection<Attendance>();
        }

        public Notification Cancel(ICollection<ApplicationUser> followers)
        {
            IsCanceled = true;

            var notification = Notification.PictureCanceled(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }

            foreach (var follower in followers)
            {
                follower.Notify(notification);
            }

            return notification;
        }

        public Notification Modify(DateTime dateTime, string venue, byte category, ICollection<ApplicationUser> followers)
        {
            var notification = Notification.PictureUpdated(this, Venue);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }

            foreach (var follower in followers)
            {
                follower.Notify(notification);
            }

            Venue = venue;
            DateTime = dateTime;
            CategoryId = category;

            return notification;
        }
    }

}
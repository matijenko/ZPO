using System;
using System.ComponentModel.DataAnnotations;

namespace work.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public string OriginalVenue { get; private set; }
        public ApplicationUser Attender { get; private set; }

        [Required]
        public Picture Picture { get; private set; }

        protected Notification()
        {

        }

        private Notification(NotificationType type, Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            Type = type;
            Picture = picture;
            DateTime = DateTime.Now;
        }

        private Notification(NotificationType type, Picture picture, ApplicationUser user)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            Type = type;
            Picture = picture;
            DateTime = DateTime.Now;
            Attender = user;
        }

        public static Notification PictureBought(Picture picture, ApplicationUser attender)
        {
            return new Notification(NotificationType.PictureBought, picture, attender);
        }

        public static Notification PictureCreated(Picture picture)
        {
            return new Notification(NotificationType.PictureCreated, picture);
        }

        public static Notification PictureUpdated(Picture newPicture, string originalVenue)
        {
            var notification = new Notification(NotificationType.PictureUpdated, newPicture);
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification PictureCanceled(Picture picture)
        {
            return new Notification(NotificationType.PictureCanceled, picture);
        }
    }
}
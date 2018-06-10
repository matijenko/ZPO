using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using work.Models;

namespace work.Controllers.Api
{
    [Authorize]
    public class PicturesController : ApiController
    {
        private ApplicationDbContext _context;

        public PicturesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var picture = _context.Pictures
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id && g.ArtistId == userId);
            if (picture.IsCanceled)
                return NotFound();

            var notification = picture.Cancel(_context.Followings.Where(x => x.FolloweeId == userId).Select(x => x.Follower).ToList());
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return Ok();
        }
    }
}

using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using work.Controllers.Api;
using work.Models;

namespace work.Controllers
{
    [Authorize]
    public class FollowingController : ApiController
    {
        private ApplicationDbContext _context;

        public FollowingController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(string id)
        {
            var userId = User.Identity.GetUserId();

            if (_context.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == id))
                return BadRequest("Following already exists.");

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = id
            };
            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult UnFollow(string id)
        {
            var userId = User.Identity.GetUserId();

            var following = _context.Followings.Single(x => x.FollowerId == userId && x.FolloweeId == id);

            _context.Followings.Remove(following);
            _context.SaveChanges();

            return Ok();
        }

    }
}

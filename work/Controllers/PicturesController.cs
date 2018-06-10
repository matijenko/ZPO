using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using work.Models;
using work.ViewModels;

namespace work.Controllers
{

    public class PicturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PicturesController()
        {
            _context = new ApplicationDbContext();
        }
        [Authorize]
        public void BuyPicture(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Find(userId);
            _context.Attendances.Add(new Attendance() { PictureId = id, AttendeeId = userId });
            var notification = Notification.PictureBought(_context.Pictures.Find(id), user);
            var picture = _context.Pictures.Find(id);
            _context.Notifications.Add(notification);
            _context.Users.Find(picture.ArtistId).Notify(notification);
            _context.SaveChanges();          
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var pictures = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Picture)
                .Include(g => g.Artist)
                .Include(g => g.Category)
                .OrderByDescending(x => x.DateTime)
                .ToList();

            var followed = _context.Followings.Where(x => x.FollowerId == userId).ToList();

            foreach (var picture in pictures)
            {
                picture.IsFollowed = followed.Any(x => x.FolloweeId == picture.ArtistId);
            }

            var viewModel = new HomeViewModel()
            {
                UpcomingPictures = pictures,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Pictures I'm Attending"
            };

            return View("Attending", viewModel);
        }

        [HttpPost]
        public ActionResult Search(HomeViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new {query = viewModel.SearchTerm});
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var pictures = _context.Pictures
                .Where(g => g.ArtistId == userId 
                            && !g.IsCanceled)
                .Include(g => g.Category).
                OrderByDescending(x => x.DateTime)
                .ToList();

            return View("Mine", pictures);
        }

        public FileContentResult Show(int id)
        {
            var image = _context.Pictures.Single(c => c.Id == id);
            byte[] imageByte = image.ImageContent;
            string contentType = image.ContentType;
            WebImage img = new WebImage(imageByte);
            img = img.Resize(300, 300);
            return File(img.GetBytes(), "image/jpeg;");
        }


        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new PictureFormViewModel
            {
                Categories = _context.Categories.ToList(),
                Heading = "Add a Picture"
            };
           return View("PictureForm",viewModel);
        }

        [Authorize]
        public ActionResult CreateEdit()
        {
            var viewModel = new PictureEditFormViewModel
            {
                Categories = _context.Categories.ToList(),
                Heading = "Edit a Picture"
            };
            return View("Edit", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var picture = _context.Pictures.Single(g => g.Id == id && g.ArtistId == userId);
       
            var viewModel = new PictureEditFormViewModel
            {
                Heading = "Edit a Picture",
                Id = picture.Id,
                Categories = _context.Categories.ToList(),
                Category = picture.CategoryId,
                Venue = picture.Venue
            };
            return View("Edit", viewModel);
        }

        public ActionResult Category(int id)
        {
            var userId = User?.Identity?.GetUserId();
            var images = !string.IsNullOrEmpty(userId) ? _context.Pictures.Where(c => c.CategoryId == id && c.ArtistId != userId && !c.Attendances.Any(x => x.AttendeeId == userId) && !c.IsCanceled).Include(x => x.Category).Include(x => x.Artist).OrderByDescending(x => x.DateTime).ToList()
                : _context.Pictures.Where(c => c.CategoryId == id && !c.IsCanceled).Include(x => x.Category).Include(x => x.Artist).OrderByDescending(x => x.DateTime).ToList();

            var viewModel = new CategoryViewModel
            {
                Heading = _context.Categories.Find(id)?.Name,
                Pictures = images
            };


            return View("Categories", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PictureFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _context.Categories.ToList();
                return View("PictureForm", viewModel);

            }

            BinaryReader br = new BinaryReader(viewModel.File.InputStream);
            Byte[] bytes = br.ReadBytes((Int32)viewModel.File.InputStream.Length);

            var userId = User.Identity.GetUserId();


            var picture = new Picture
            {
                ArtistId = userId,
                DateTime = DateTime.Now,
                CategoryId = viewModel.Category,
                ContentType = viewModel.File.ContentType,
                ImageContent = bytes,
                Venue = viewModel.Venue
            };

            _context.Pictures.Add(picture);
            var notification = Notification.PictureCreated(picture);
            _context.Notifications.Add(notification);
            foreach (var follower in _context.Followings.Where(x => x.FolloweeId == userId).Select(x => x.Follower))
                follower.Notify(notification);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Pictures");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PictureEditFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _context.Categories.ToList();
                return View("Edit", viewModel);

            }

            var userId = User.Identity.GetUserId();
            var picture = _context.Pictures
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            var notification = picture.Modify(DateTime.Now, viewModel.Venue, viewModel.Category,
                _context.Followings.Where(x => x.FolloweeId == userId).Select(x => x.Follower).ToList());

            _context.Notifications.Add(notification);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Pictures");
        }

    }
}
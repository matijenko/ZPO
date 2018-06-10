using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using work.Models;
using Rotativa;

namespace work.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
           
            public ActionResult Index(string query = null)
        {
            var userId = User.Identity.GetUserId();
            var upcomingPictures = _context.Pictures
                .Include(g => g.Artist)
                .Include(g => g.Category)
                .Where(g => !g.IsCanceled)
                .OrderByDescending(x => x.DateTime)
                .Take(15);
            if (!string.IsNullOrEmpty(userId))
            {
                upcomingPictures = upcomingPictures.Where(c => c.ArtistId != userId && !c.Attendances.Any(x => x.AttendeeId == userId) && !c.IsCanceled);
            }

            if (!String.IsNullOrWhiteSpace(query))
            {
                upcomingPictures = upcomingPictures
                    .Where(g =>
                        g.Artist.Name.Contains(query) ||
                        g.Category.Name.Contains(query) ||
                        g.Venue.Contains(query));
            }

            var viewModel = new HomeViewModel
            {
                UpcomingPictures = upcomingPictures,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Pictures",
                SearchTerm = query
            };

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

         public ActionResult DownloadViewPDF()
         { 
             var model = new GeneratePDFModel();
            //Code to get content
             return new Rotativa.ViewAsPdf("GeneratePDF", model) { FileName = "TestViewAsPdf.pdf",
                 CustomSwitches =
            "--footer-center \"Name: " + "XYZ" + "  DOS: " +
            DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
            " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
             };
         } 

         public ActionResult DownloadActionAsPDF()
         {
             var model = new GeneratePDFModel();
             //Code to get content
             return new Rotativa.ActionAsPdf("GeneratePDF", model) { FileName = "TestActionAsPdf.pdf",
                 CustomSwitches =
            "--footer-center \"Name: " + "Action" + "  DOS: " +
            DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
            " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
             };
         }

         public ActionResult GeneratePDF()
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

            return new Rotativa.ViewAsPdf(viewModel)
            {
                FileName = "GeneratePdf.pdf",
                CustomSwitches =
            "--footer-center \"Name: " + "View" + "  DOS: " +
            DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
            " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
            };
        }

        public ActionResult UrlAsPDF()
         {
             return new Rotativa.UrlAsPdf("https://localhost:44392/") { FileName = "UrlTest.pdf",
                 CustomSwitches =
            "--footer-center \"Name: " + "View" + "  DOS: " +
            DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
            " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
             };
         } 

    }
}
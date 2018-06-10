using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using work.Controllers.Api;
using work.Models;

namespace work.Controllers
{
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext _context;

        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IEnumerable<CategoryDto> GetCategories()
        {
            return _context.Categories.Select(Mapper.Map<Category, CategoryDto>);
        }

    }
}
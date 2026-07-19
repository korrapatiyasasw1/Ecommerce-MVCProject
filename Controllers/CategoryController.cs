using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> Category = _db.Categories.ToList();
            return View(Category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            
            if(ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();


        }
        public IActionResult Update(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            Category category = _db.Categories.Find(id);
            if (category == null) {
                return NotFound();
                    }
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(Category obj)
        {
            
            
            if(ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(obj);

        }
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);
            return View(category);
        }
        [HttpPost ]
        public IActionResult Delete(Category category)
        {
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

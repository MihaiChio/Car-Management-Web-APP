using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly Data.ApplicationDBContext _db;

        public ApplicationTypeController(Data.ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Models.ApplicationType> appType = _db.ApplicationTypes;
            return View(appType);
        }

        //GET - Create

        public IActionResult Create()
        {
            return View();
        }

        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.ApplicationType obj)
        {
            if(ModelState.IsValid)
            {
                _db.ApplicationTypes.Add(obj);
                _db.SaveChanges();
                return Redirect("Index");
            }
            return View(obj);
        }


        //GET - EDIT
        public IActionResult Edit(int? _id) // "int?" is nullable
        {
            if(_id == null || _id == 0)
            {
                return NotFound();
            }
            Models.ApplicationType obj = _db.ApplicationTypes.Find(_id);
           if(obj == null )
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Models.ApplicationType obj)
        {
            if(ModelState.IsValid)
            {
                _db.ApplicationTypes.Update(obj);
                _db.SaveChanges();
                return Redirect("Index");
            }
            return View(obj);
        }


        //GET - Delete

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Models.ApplicationType obj = _db.ApplicationTypes.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        //post - Delete
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeletePost(int? id)
        {
            Models.ApplicationType appType = _db.ApplicationTypes.Find(id);
            if(appType == null)
            {
                return NotFound();
            }
            _db.ApplicationTypes.Remove(appType);
            _db.SaveChanges();
            return Redirect("Index");
        }

    }
}

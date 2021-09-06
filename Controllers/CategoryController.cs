using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1MVC.Data;

namespace WebApplication1MVC.Controllers
{
    public class CategoryController : Controller
    {
        //dependency injection
        private readonly ApplicationDBContext _db;

        public CategoryController(Data.ApplicationDBContext db) //constructor // pulling from the service container.
        {
            _db = db; // assigning the local db to the new one.
        }

        public IActionResult Index()
        {
            IEnumerable<Models.Category> objList = _db.Category; // gets the class from Models.Category.
            return View(objList);
        }

        //get create
        public IActionResult Create()
        {
            
            return View();
        }


        //POST - create
        [HttpPost]
        [ValidateAntiForgeryToken] //built in mechanism that check if the token is still valid and security has not been tampered.
        public IActionResult Create(Models.Category obj)
        {
            if (ModelState.IsValid) //this is server side validation. // Validation is done in the controller (CategoryController) 
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return Redirect("Index");
            }
            return View(obj);

        }

        //GET - EDIT
        public IActionResult Edit(int? _id)
        {
            //The function paramater and the ASP-ROUTE have to be the same otherwise data will not be passed. CLAPPED.
            //Check Index for details. 
            //E.g. asp-route-_id => function paramters must be called "_id".
            if(_id==null || _id == 0)
            {
                return NotFound();
            }
            Models.Category obj = _db.Category.Find(_id); //only works with primary key. whatever is passed in will be compared against the primary keys column.
            if(obj == null)
            {
                return NotFound();
            }
                return View(obj);

        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Models.Category obj)
        {
            if (ModelState.IsValid) //this is server side validation. // Validation is done in the controller (CategoryController) 
            {
                _db.Category.Update(obj);
                _db.SaveChanges();
                return Redirect("Index");
            }
            return View(obj);

        }

        //Delete - GET
        public IActionResult Delete(int? id)
        {
            //The function paramater and the ASP-ROUTE have to be the same otherwise data will not be passed. CLAPPED.
            //Check Index for details. 
            //E.g. asp-route-_id => function paramters must be called "_id".
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Models.Category obj = _db.Category.Find(id); //only works with primary key. whatever is passed in will be compared against the primary keys column.
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            Models.Category obj = _db.Category.Find(id);
            if(obj==null)
            {
                return NotFound();
            }
                _db.Category.Remove(obj);
                _db.SaveChanges();
                return Redirect("Index");

        }

    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1MVC.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1MVC.Controllers
{
    public class ProductController : Controller
    {
        //dependency injection
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(Data.ApplicationDBContext db, IWebHostEnvironment webHostEnvironment) //constructor // pulling from the service container.
        {
            _db = db; // assigning the local db to the new one.
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //eager loading
            IEnumerable<Models.Product> objList = _db.Product.Include(u => u.category).Include(u => u.applicationType); // gets the class from Models.Category.
            //IEnumerable contains in this case all the objects of type product from the database.
            //this is useful as we can loop through them.

            // this should be replaced with eager loading as too many query calls are made.
            //foreach(Models.Product obj in objList)
            //{
            //    obj.category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryID);
            //    obj.applicationType = _db.ApplicationTypes.FirstOrDefault(u => u.iD == obj.ApplicationID);
            //    //goes through all objects, loads the category model.
            //}
            return View(objList);
        }

        //get upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem //retrieving every category in the database and converting them to a new selectListItem
            //{
            //    Text = i.name,
            //    Value = i.Id.ToString()
            //    //retrieving the 
            //});

            //ViewBag.CategoryDropwDown = CategoryDropwDown; // ideal for temporary data that has to be passed from the controller to the view.

            //view bag's lifetime will only last during the current http request.
            // viewBag transfers data from the controller to the view, not vice versa.
            //This is ideal for situations in which the temporary data is not in a model

            ////////view data - dictiondary.//////////////////////////////////////////////////////////////////////////////////////////////////////

            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //ViewData - transfers data from controller to view. Not vice versa
            //ideal for temporary data
            //ViewData is derived from a ViewDataDictionary
            //Must be type cast before use.
            //only last during the current http request.
            //will be null if redirection occurs.

            Models.ViewModels.productVM productVM = new Models.ViewModels.productVM()
            {
                product = new Models.Product(),
                categorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.name,
                    Value = i.Id.ToString()
                }),

                applicationSelectList = _db.ApplicationTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.iD.ToString()
                })
            };

            Models.Product product = new Models.Product();
            if(id == null)
            {
                //this is gonna create a new product
                return View(productVM);
            }
            else
            {
                //this is not null therefore we just want to edit.
                productVM.product = _db.Product.Find(id);
                if(productVM.product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }

        }


        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken] //built in mechanism that check if the token is still valid and security has not been tampered.
        public IActionResult Upsert(Models.ViewModels.productVM productVM)
        {
            if (ModelState.IsValid) //this is server side validation. // Validation is done in the controller (ProductController) 
            {
                var files = HttpContext.Request.Form.Files; // this is to retrieve the newly uploaded images.
                string webRootPath = _webHostEnvironment.WebRootPath; // this stores the path to the "wwwroot" folder.

                if(productVM.product.id == 0)
                {
                    //Creating the product.
                    string upload = webRootPath + WebConstants.imagePath;//this will get the path to the folder where we want to save the image on the server.
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName); // getting the extension of the file.

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                        //copying the file to the new location which is upload.
                    }

                    //Updating the product to the image.
                    productVM.product.imagePath = fileName + extension;

                    _db.Product.Add(productVM.product);
                }
                else
                {
                    //updating
                    Models.Product objFromDB = _db.Product.AsNoTracking().FirstOrDefault(u => u.id == productVM.product.id);
                    //retrieving the object from the database. Everything will be stored in obj.
                    //"AsNoTracking()" does not track the object in terms of updates. Basically tells the compiler to not update that object.
                    if (files.Count > 0)
                    {
                        //new file has been updated.
                        string upload = webRootPath + WebConstants.imagePath;//this will get the path to the folder where we want to save the image on the server.
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName); // getting the extension of the file. 



                        //remove the old Image file.
                        var oldFile = Path.Combine(upload, objFromDB.imagePath);

                        //checking if the file exist
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                            //copying the file to the new location which is upload.
                        }

                        productVM.product.imagePath = fileName + extension;
                    }
                    else
                    {
                        productVM.product.imagePath = objFromDB.imagePath; // image stays the same if not modified.
                    }
                    _db.Product.Update(productVM.product);
                }

                _db.SaveChanges();
                return RedirectToAction(actionName:"Index",controllerName:"Product");
            }

            productVM.categorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.name,
                Value = i.Id.ToString()
            });
            productVM.applicationSelectList = _db.ApplicationTypes.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.iD.ToString()
            });
            //this is to avoid getting exceptions if the model is not valid.
            return View(productVM);

        }

        //Delete - GET
        public IActionResult Delete(int? id)
        {
            //The function paramater and the ASP-ROUTE have to be the same otherwise data will not be passed. CLAPPED.
            //Check Index for details. 
            //E.g. asp-route-_id => function paramters must be called "_id".

            Models.Product product = _db.Product.Include(u => u.category).Include(u => u.applicationType).FirstOrDefault(u=> u.id == id);
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                product = _db.Product.Find(id);
                return View(product);
            }

        }

        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            Models.Product product = _db.Product.Find(id);
            if(id==null)
            {
                return NotFound();
            }
            //deleting the image
            string localPath = _webHostEnvironment.WebRootPath + WebConstants.imagePath;
            var deleteFilePath = Path.Combine(localPath, product.imagePath);
            if(System.IO.File.Exists(deleteFilePath))
            {
                System.IO.File.Delete(deleteFilePath);
            }

            //

                _db.Product.Remove(product);
                _db.SaveChanges();
                return RedirectToAction(actionName: "Index", controllerName: "Product");

        }

    }
}

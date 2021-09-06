using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WebApplication1MVC.Models
{
    public class Product
    {
        [Key]

        public int id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string name { get; set; }

        //this is to demonstrate model editing.
        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }

        //

        [DisplayName("Description")]
        public string description { get; set; }

        [Range(1,int.MaxValue)]
        [DisplayName("Price")]
        public double price { get; set; }

        public string imagePath { get; set; }
        //this stores the image path
        //explicitly add the mapping entity.


        [Display(Name = "Category Type")]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]

        //configure foreign key relation Category ID
        public virtual Category category { get; set; }
        //virtual keyword allows for the above method to be overriden by any class that inherits it.
        //this automatically adds a mapping between product and category. It also creates a category ID column. Column is not visible.

        [Display(Name = "Application Type")]
        public int ApplicationID { get; set; }
        [ForeignKey("ApplicationID")]
        public virtual ApplicationType applicationType { get; set; }
    }
}

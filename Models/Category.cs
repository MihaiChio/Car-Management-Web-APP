using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1MVC.Models
{
    //this is the category model.
    public class Category
    {
        [Key]

        public int Id { get; set; } //this collumn is a primary key and is used as identity for the table.
        
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }
        
        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Display order must be greater than 0")]
        [DisplayName("Display Order")]
        public int displayOrder { get; set; }

        //TODO -> create a table in the database and add these three columns in that table.
    }
}

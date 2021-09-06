using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Models
{
    public class ApplicationType
    {
        [Key]
        public int iD { get; set; } // primary key

        [Required]
        public string Name { get; set; }
    }
}

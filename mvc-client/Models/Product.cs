using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mvc_client.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public int CateID { get; set; }

        [Required(ErrorMessage = "Product Name cannot be empty")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product Price must be greater than zero")]
        [DisplayName("Product Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Product Price must be greater than zero")]
        [DisplayName("Quantity")]
        public int Stock { get; set; }

        public virtual Category Category { get; set; }
    }
}
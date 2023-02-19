using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_server.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("CategoryName")]
        public string Name { get; set; }

        [ForeignKey("CateID")]
        public virtual ICollection<Product> Product { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace BookPracEFcore
{
    internal class Category
    {
        [Key]
        public int CategoryId {  get; set; }

        [Required]
        //[StringLength(15)]
        public string? CategoryName { get; set; }

        [Column(TypeName ="ntext")]
        public string? Description { get; set; }

        // 네비게이션 프로퍼티
        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}

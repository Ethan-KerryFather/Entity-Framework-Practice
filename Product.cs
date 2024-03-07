using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPracEFcore
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; } // 기본키

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; } = null!; // 이 ProductName은 실행하면 Null이 아닐겁니다.

        [Column("UnitPrice", TypeName = "money")]
        public decimal? Cost { get; set; }

        [Column("UnitsInStock")]
        public short? Stock { get; set; }

        public bool Discontinued { get; set; } // bit

        // 네비게이션 프로퍼티
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!; 
        // ef core에서 네비게이션 프로퍼티를 virtual로 선언하는 것은 해당 프로퍼티가 지연 로딩될 수 있음을 의미한다.
        // 지연로딩 : 관련된 엔터티를 실제로 접근할 때까지 로딩을 지연하는 것. 
    }
}

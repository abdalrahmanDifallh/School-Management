using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        //[Column(Order = 1)]
        //public string Code { get; set; }

        [Column(Order = 1000)]
        public DateTime? CreatedAt { get; set; }

        [Column(Order = 1001)]
        public DateTime? UpdatedAt { get; set; }

        [Column(Order = 1002)]
        public string? CreatedBy { get; set; }

        [Column(Order = 1003)]
        public string? UpdatedBy { get; set; }

        [Column(Order = 1004)]
        public bool? IsActive { get; set; } = true;

        [Column(Order = 1005)]
        public bool? IsDeleted { get; set; }

    }

    public class PagedListResult<T>
    {
        public List<T> Result { get; set; }
        public int Count { get; set; }
    }
}

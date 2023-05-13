using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DB_test
{
    [Table("Main")]
    public class Main
    {
        [Key]
        [Column("url",TypeName ="varchar")]
        public string Url { get; set; }
        [Column("isOpen",TypeName = "bit")]
        public bool isOpen { get; set; }

        public Main(string URL, bool isOpen) { Url = URL; this.isOpen = isOpen; }
    }
}

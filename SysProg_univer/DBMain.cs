using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SysProg_univer
{
    [Table("Record")]
    public class DbRecord
    {
        [Key]
        [Column("url",TypeName ="varchar")]
        public string Url { get; set; }
        [Column("isOpen",TypeName = "bit")]
        public bool isOpen { get; set; }

        public DbRecord() { }
        
        public DbRecord(RecordLocal record) { Url = record.url; isOpen = record.isOpen; }
        public DbRecord(string URL, bool isOpen) { Url = URL; this.isOpen = isOpen; }
    }
}

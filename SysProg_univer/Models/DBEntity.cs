using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SysProgUniver
{
    [Table("Record")]
    public class Record
    {

        [Key]
        [Column("url",TypeName ="varchar")]
        public string Url { get; set; }
        [Column("isOpen", TypeName = "bit")]
        public bool IsOpen { get; set; }
        [Column("CheckedAt", TypeName = "datetime")]
        public DateTime CheckedAt { get; set; }

        public Record() { }
        public Record(string url, bool isOpen, DateTime dt) { Url = url; this.IsOpen = isOpen; this.CheckedAt = dt; }
        public Record(string url, bool isOpen) { Url = url; this.IsOpen = isOpen; this.CheckedAt = DateTime.Now; }

        public static Record[] Extract(string text)
        {
            if (text == null)
                return null;
            var pattern = @"(URI: (.*)\nOpen: (True|False)\nDT: (\d\d?\/\d\d?\/\d\d\d\d\s\d\d?:\d\d?:\d\d?)[\s\r\n\]?)|(^[\s\n\r]*)";
            List<Record> records = new List<Record>();
            if (text.Split('\n').Length % 3 != 0 || Regex.Replace(text, pattern, "").Length != 0)
                throw new ArgumentOutOfRangeException("text", "Одна или несколько записей содержат неполные данные.");
            var matches = Regex.Matches(text, pattern);
            foreach (Match match in matches)
            {
                if (match.Success && !Regex.Match(match.Groups[2].Value, @"^[\s\n\r]*$").Success)
                    records.Add(new Record(match.Groups[2].Value, match.Groups[3].Value == "True", DateTime.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture)));
            }
            return records.ToArray();
        }
        public static string Compress(Record[] record)
        {
            string ret = "";
            int asd = 0;
            if (record != null)
                foreach (Record r in record)
                {
                    ret += r.ToString();
                    if (asd + 1 < record.Length)
                        ret += '\n';
                    asd++;
                }
            return ret;
        }

        public override string ToString()
        {
            return
                $"URI: {this.Url}\n" +
                $"Open: {this.IsOpen}\n" +
                $"DT: {this.CheckedAt.ToString(CultureInfo.InvariantCulture)}";
        }

    }
}

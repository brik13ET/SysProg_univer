using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SysProg_univer
{
    [Table("Record")]
    public class Record
    {

        [Key]
        [Column("url",TypeName ="varchar")]
        public string Url { get; set; }
        [Column("isOpen",TypeName = "bit")]
        public bool isOpen { get; set; }

        public Record() { }
        public Record(string URL, bool isOpen) { Url = URL; this.isOpen = isOpen; }

        public static Record[] Extract(string text)
        {
            var pattern = @"(URI: (.*)\nOpen: (True|False)\n?)|(^[\s\n\r]*$)";
            List<Record> records = new List<Record>();
            if (text.Split('\n').Length % 2 == 1 || Regex.Replace(text, pattern, "").Length != 0)
                throw new ArgumentOutOfRangeException("text", "Одна или несколько записей содержат неполные данные.");
            var matches = Regex.Matches(text, pattern);
            int asodn = 0;
            foreach (Match match in matches)
            {
                Console.WriteLine(asodn++);
                int alskd = 0;
                foreach (Group grp in match.Groups)
                {
                    Console.WriteLine("\t{0}: `{1}`", alskd++, grp.Value.Replace("\n", "\\n"));
                }
                if (match.Success && !Regex.Match(match.Groups[2].Value, @"^[\s\n\r]*$").Success)
                    records.Add(new Record(match.Groups[2].Value, match.Groups[3].Value == "True"));
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

    }
}

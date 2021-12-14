using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Models
{
    public class CommentUser
    {
        public string Comment { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public CommentUser(string comment, string name, DateTime date, int count)
        {
            Comment = comment;
            Name = name;
            Date = date;
            Count = count;
        }
    }
}

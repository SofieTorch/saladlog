using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Models
{
    public class ArticleDataView
    {
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string NameUserComment { get; set; }

        public ArticleDataView()
        {

        }

        public ArticleDataView(DateTime dateCreate, string title, string content, string nameUserComment)
        {
            DateCreate = dateCreate;
            Title = title;
            Content = content;
            NameUserComment = nameUserComment;
        }
    }
}

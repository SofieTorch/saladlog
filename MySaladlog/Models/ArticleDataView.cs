using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaladlog.Models
{
    public class ArticleDataView
    {
        public short IdArticle { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string NameUserComment { get; set; }
        public short IdUser { get; set; }

        public ArticleDataView()
        {

        }

        public ArticleDataView(short idArticle, DateTime dateCreate, string title, string content, string nameUserComment, short idUser)
        {
            IdArticle = idArticle;
            DateCreate = dateCreate;
            Title = title;
            Content = content;
            NameUserComment = nameUserComment;
            IdUser = idUser;
        }
    }
}

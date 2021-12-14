using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Table("Tag_Article")]
    public partial class TagArticle
    {
        [Column("idTag")]
        public short IdTag { get; set; }
        [Column("idArticle")]
        public short IdArticle { get; set; }
        [Key]
        [Column("idTagArticle")]
        public int IdTagArticle { get; set; }

        [ForeignKey(nameof(IdArticle))]
        [InverseProperty(nameof(Article.TagArticles))]
        public virtual Article IdArticleNavigation { get; set; }
        [ForeignKey(nameof(IdTag))]
        [InverseProperty(nameof(Tag.TagArticles))]
        public virtual Tag IdTagNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Table("Like_Article")]
    public partial class LikeArticle
    {
        [Column("idUser")]
        public short IdUser { get; set; }
        [Column("idArticle")]
        public short IdArticle { get; set; }
        [Key]
        [Column("idLikeArticle")]
        public int IdLikeArticle { get; set; }

        [ForeignKey(nameof(IdArticle))]
        [InverseProperty(nameof(Article.LikeArticles))]
        public virtual Article IdArticleNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.LikeArticles))]
        public virtual User IdUserNavigation { get; set; }
    }
}

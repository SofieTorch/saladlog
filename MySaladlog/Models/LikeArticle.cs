using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Keyless]
    [Table("Like_Article")]
    public partial class LikeArticle
    {
        [Column("idUser")]
        public short IdUser { get; set; }
        [Column("idArticle")]
        public short IdArticle { get; set; }

        [ForeignKey(nameof(IdArticle))]
        public virtual Article IdArticleNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        public virtual User IdUserNavigation { get; set; }
    }
}

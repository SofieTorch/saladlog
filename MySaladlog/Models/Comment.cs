using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    public partial class Comment
    {
        [Key]
        [Column("idComments")]
        public short IdComments { get; set; }
        [Required]
        [Column("comment")]
        [StringLength(1500)]
        public string Comment1 { get; set; }
        [Column("idArticle")]
        public short IdArticle { get; set; }
        [Column("idUser")]
        public short IdUser { get; set; }
        [Column("dateComment", TypeName = "datetime")]
        public DateTime DateComment { get; set; } = DateTime.Now;

        [ForeignKey(nameof(IdArticle))]
        [InverseProperty(nameof(Article.Comments))]
        public virtual Article IdArticleNavigation { get; set; }
        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.Comments))]
        public virtual User IdUserNavigation { get; set; }
    }
}

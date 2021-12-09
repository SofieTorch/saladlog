using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Table("Article")]
    public partial class Article
    {
        public Article()
        {
            Comments = new HashSet<Comment>();
        }

        [Key]
        [Column("idArticle")]
        public short IdArticle { get; set; }
        [Column("idUser")]
        public short IdUser { get; set; }
        [Required]
        [Column("title")]
        [StringLength(100)]
        public string Title { get; set; }
        [Column("createDate", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Required]
        [Column("path")]
        [StringLength(250)]
        public string Path { get; set; }
        [Column("reportsNumber")]
        public byte ReportsNumber { get; set; }

        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.Articles))]
        public virtual User IdUserNavigation { get; set; }
        [InverseProperty(nameof(Comment.IdArticleNavigation))]
        public virtual ICollection<Comment> Comments { get; set; }

        [NotMapped]
        [BindProperty]
        public string MdContent { get; set; } = string.Empty;
    }
}

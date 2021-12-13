using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Articles = new HashSet<Article>();
            Comments = new HashSet<Comment>();
            LikeArticles = new HashSet<LikeArticle>();
        }

        [Key]
        [Column("idUser")]
        public short IdUser { get; set; }
        [Required]
        [Column("firstName")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Column("lastName")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [Column("userName")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [Column("password")]
        [StringLength(50)]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

        [InverseProperty(nameof(Article.IdUserNavigation))]
        public virtual ICollection<Article> Articles { get; set; }
        [InverseProperty(nameof(Comment.IdUserNavigation))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(LikeArticle.IdUserNavigation))]
        public virtual ICollection<LikeArticle> LikeArticles { get; set; }
    }
}

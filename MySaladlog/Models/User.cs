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
        }

        [Key]
        [Column("idUser")]
        public short IdUser { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Column("firstName")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [Column("lastName")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [Column("userName")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Column("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion de contraseña es requerida")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El correo electronico es requerido")]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

        [InverseProperty(nameof(Article.IdUserNavigation))]
        public virtual ICollection<Article> Articles { get; set; }
        [InverseProperty(nameof(Comment.IdUserNavigation))]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            TagArticles = new HashSet<TagArticle>();
        }

        [Key]
        [Column("idTag")]
        public short IdTag { get; set; }
        [Required]
        [Column("tagName")]
        [StringLength(30)]
        public string TagName { get; set; }


        [InverseProperty(nameof(TagArticle.IdTagNavigation))]
        public virtual ICollection<TagArticle> TagArticles { get; set; }

    }

   
}

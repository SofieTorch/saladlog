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
        [Key]
        [Column("idTag")]
        public short IdTag { get; set; }
        [Required]
        [Column("tagName")]
        [StringLength(30)]
        public string TagName { get; set; }

        public Tag(short idTag, string tagName)
        {
            IdTag = idTag;
            TagName = tagName;
        }
    }

   
}

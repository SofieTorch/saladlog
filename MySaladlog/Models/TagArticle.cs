using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MySaladlog.Models
{
    [Keyless]
    [Table("Tag_Article")]
    public partial class TagArticle
    {
        [Column("idTag")]
        public short IdTag { get; set; }
        [Column("idArticle")]
        public short IdArticle { get; set; }

        [ForeignKey(nameof(IdArticle))]
        public virtual Article IdArticleNavigation { get; set; }
        [ForeignKey(nameof(IdTag))]
        public virtual Tag IdTagNavigation { get; set; }
    }
}

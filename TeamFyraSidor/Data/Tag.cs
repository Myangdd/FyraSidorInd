using System.ComponentModel.DataAnnotations;

namespace TeamFyraSidor.Data
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        public virtual List<Article>? Articles { get; set; }
    }
}

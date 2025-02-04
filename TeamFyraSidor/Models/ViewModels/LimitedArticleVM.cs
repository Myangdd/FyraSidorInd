using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Models.ViewModels
{
    public class LimitedArticleVM
    {
        public int Id { get; set; }
        [DisplayName("Date Stamp")]
        public DateTime DateStamp { get; set; } = DateTime.Now;
        [StringLength(300)]
        public string? LinkText { get; set; }

        [Required]
        [StringLength(150)]
        public string Headline { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [DisplayName("Content Summary")]
        public string ContentSummary { get; set; } = string.Empty;
                
        public int Views { get; set; }
        public int Likes { get; set; }
        public string ImageLink { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsArchived { get; set; }

        [Required]
        [StringLength(200)]
        public string Author { get; set; } = string.Empty;
        public DateTime UpdateStamp { get; set; }
        public List<Tag>? Tags { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }


    }
}

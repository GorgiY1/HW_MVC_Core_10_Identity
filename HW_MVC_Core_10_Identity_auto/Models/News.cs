using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HW_MVC_Core_10_Identity.Models
{
    public class News
    {
        [Key]
        public int ArticleId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string AuthorId { get; set; } = string.Empty; // Связь с автором (пользователем)

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Навигационное свойство для связи с пользователем
        public IdentityUser? Author { get; set; }
    }

}

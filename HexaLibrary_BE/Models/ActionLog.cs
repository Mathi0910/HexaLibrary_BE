using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexaLibrary_BE.Models
{
    public class ActionLog
    {
        [Key]
        public int LogId { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required]
        public DateTime ActionTime { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string? Details { get; set; }

        

    }
}

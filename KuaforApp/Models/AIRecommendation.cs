using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuaforApp.Models
{
    public class AIRecommendation
    {
        [Key]
        public int RecommendationID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string PhotoPath { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string RecommendedStyles { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation property
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
    }
} 
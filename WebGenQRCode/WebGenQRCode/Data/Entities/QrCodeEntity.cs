using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebGenQRCode.Data.Entities.Identity;

namespace WebGenQRCode.Data.Entities
{
    public class QrCodeEntity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, StringLength(100)]
        public string Code { get; set; } = null!;

        public string TargetUrl { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public int ScanCount { get; set; }

    }
}

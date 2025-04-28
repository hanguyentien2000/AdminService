using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Insfrastructure.Databases
{
    [Table("idm_Users")]
    public partial class IdmUsers
    {
        public IdmUsers()
        {
;
        }
        [Key]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(512)]
        public string NickName { get; set; }
        [MaxLength(128)]
        public string MobilePin { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string OtherEmail { get; set; }
        public int Type { get; set; }
        [MaxLength(500)]
        public string FullName { get; set; }
        [MaxLength(500)]
        public string ShortName { get; set; }
        public DateTime? Birthday { get; set; }
        [Required]
        [StringLength(256)]
        public string LoweredUserName { get; set; }
        [MaxLength(128)]
        public string IdentityNumber { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        [Required]
        [StringLength(128)]
        public string PasswordSalt { get; set; }
        public bool IsLockedOut { get; set; }

        [MaxLength(1024)]
        public string Avatar { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string IdentityAddress { get; set; }
        [MaxLength(1)]
        public string Gender { get; set; }

        // Refresh Token fields
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public bool IsRefreshTokenRevoked { get; set; }

        //base
        public Guid? CreatedByUserId { get; set; }
        public string CreatedByFullName { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public Guid? LastModifiedByUserId { get; set; }
        public string LastModifiedByFullName { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

        public virtual ICollection<IdmUsersInRoles> IdmUsersInRoles { get; set; }
    }
}

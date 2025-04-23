using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminService.Insfrastructure.Databases
{
    [Table("idm_Roles")]
    public partial class IdmRoles
    {
        public IdmRoles()
        {

        }

        public Guid ApplicationId { get; set; }
        [Key]
        public Guid RoleId { get; set; }
        [Required]
        [StringLength(256)]
        public string RoleName { get; set; }
        [Required]
        [StringLength(256)]
        public string LoweredRoleName { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        public bool EnableDelete { get; set; }
        [Column("CreatedByUserID")]
        public Guid CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }
        [Column("LastModifiedByUserID")]
        public Guid LastModifiedByUserId { get; set; }
        public string LastModifiedByFullName { get; set; }

        public DateTime LastModifiedOnDate { get; set; }
        [StringLength(256)]
        public string RoleCode { get; set; }
    }
}

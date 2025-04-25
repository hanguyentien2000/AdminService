using System.ComponentModel.DataAnnotations.Schema;

namespace AdminService.Insfrastructure.Databases
{
    [Table("idm_UsersInRoles")]
    public partial class IdmUsersInRoles
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }


        public DateTime? CreateDate { get; set; }

        public DateTime? DeleteDate { get; set; }
        public Guid ApplicationId { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdmRoles Role { get; set; }
        [ForeignKey("UserId")]
        public virtual IdmUsers User { get; set; }
    }
}

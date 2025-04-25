using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Business.Roles
{
    public class RoleModel
    {
        public Guid ApplicationId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
        public bool EnableDelete { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public Guid LastModifiedByUserId { get; set; }
        public string LastModifiedByFullName { get; set; }
        public DateTime LastModifiedOnDate { get; set; }
        public string RoleCode { get; set; }
    }
}

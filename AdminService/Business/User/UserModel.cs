using AdminService.Insfrastructure.Databases;
using DataUtils;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Business.User
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public string NickName { get; set; }
        public string MobilePin { get; set; }
        public string Email { get; set; }
        public string OtherEmail { get; set; }
        public int Type { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public DateTime? Birthday { get; set; }
        public string LoweredUserName { get; set; }
        public string IdentityNumber { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsLockedOut { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string IdentityAddress { get; set; }
        public string Gender { get; set; }

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


using Constants;

namespace Models.DTO
{
    public class ChangeUserRoleDto
    {
        public int UserId { get; set; }
        public RoleEnum NewRole { get; set; }
    }
}

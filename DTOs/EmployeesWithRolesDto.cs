using API.Entities;

namespace API.DTOs
{
    public class EmployeesWithRolesDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? ManagerId { get; set; } = null;
        public List<AppRole> Roles { get; set; }
    }
}

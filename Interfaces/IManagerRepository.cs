using API.DTOs;

namespace API.Interfaces
{
    public interface IManagerRepository
    {
        Task<IEnumerable<EmployeesWithRolesDto>> GetEmployeesWithRoles();
    }
}

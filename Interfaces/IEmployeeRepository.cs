using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> SaveAllAsync();
        Task<ActionResult> AddNewEmployee(NewEmployeeDto newEmployeeDto);
        Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(int Id);
    }
}

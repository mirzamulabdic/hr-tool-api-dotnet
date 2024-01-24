using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> SaveAllAsync();
        Task<ActionResult> AddNewEmployee(NewEmployeeDto newEmployeeDto);
        Task<EmployeeDto> GetEmployeeByIdAsync(int Id);
        Task<LeaveBalanceDto> GetLeaveBalance(int EmployeeID);
    }
}

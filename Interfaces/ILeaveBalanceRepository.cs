using API.DTOs;
using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance> UpdateLeaveBalance(int leaveBalanceId, LeaveTypeEnum leaveType, int days);
    }
}

using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ILeaveBalanceRepository
    {
        void UpdateLeaveBalance(int leaveBalanceId, string leaveType, int days);
    }
}

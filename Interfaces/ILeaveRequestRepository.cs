using API.DTOs;
using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<bool> SaveAllAsync();
        void CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto);
        void CancelLeaveRequest(LeaveRequest leaveRequest);
        void ReviewLeaveRequest(int leaveRequestId, LeaveStatusEnum leaveStatus);
        Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsAsync(int UserId);
        Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsForMyEmployeesAsync(int ManagerId);
        Task<LeaveRequest> GetLeaveRequestAsync(int LeaveRequestId);

        Task UpdateTakenLeaveRequests();
        Task<IEnumerable<LeaveRequestDto>> GetLeaveEventsToday();

    }
}

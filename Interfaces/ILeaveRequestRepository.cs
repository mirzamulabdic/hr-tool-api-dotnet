using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<bool> SaveAllAsync();
        void CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto);
        void CancelLeaveRequest(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsAsync(int UserId);
        Task<LeaveRequest> GetLeaveRequestAsync(int LeaveRequestId);

    }
}

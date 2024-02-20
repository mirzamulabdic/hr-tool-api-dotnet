using API.DTOs;
using API.Entities;
using API.Enums;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<bool> SaveAllAsync();
        void CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto);
        void CancelLeaveRequest(LeaveRequest leaveRequest);
        void ReviewLeaveRequest(int leaveRequestId, LeaveStatusEnum leaveStatus);
        Task<PagedList<LeaveRequestDto>> GetMyLeaveRequestsAsync(UserParams userParams);
        Task<IEnumerable<LeaveRequestDto>> GetAllMyLeaveRequestsWithoutPagiantionAsync(int EmployeeId);
        Task<PagedList<LeaveRequestDto>> GetLeaveRequestsForMyEmployeesAsync(int ManagerId, UserParams userParams);
        Task<LeaveRequest> GetLeaveRequestAsync(int LeaveRequestId);

        Task UpdateTakenLeaveRequests();
        Task<IEnumerable<LeaveRequestDto>> GetLeaveEventsToday();

    }
}

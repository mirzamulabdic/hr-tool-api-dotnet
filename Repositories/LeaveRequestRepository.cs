using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly DataContext _dataContext;

        public LeaveRequestRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async void CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto)
        {
            var leaveRequest = new LeaveRequest
            {
                LeaveType = newLeaveRequestDto.LeaveType,
                StartDate = newLeaveRequestDto.StartDate,
                EndDate = newLeaveRequestDto.EndDate,
                DurationDays = newLeaveRequestDto.DurationDays,
                Comment = newLeaveRequestDto.Comment,
                LeaveStatus = "Pending",
                LeaveSubmitterId = newLeaveRequestDto.UserId,
                LeaveReviewerId = 1
            };

            await this._dataContext.LeaveRequests.AddAsync(leaveRequest);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public LeaveRequestRepository(DataContext dataContext, IMapper mapper)
        {
            this._dataContext = dataContext;
            this._mapper = mapper;
        }

        public void CancelLeaveRequest(LeaveRequest leaveRequest)
        {
            _dataContext.LeaveRequests.Remove(leaveRequest);
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

            await _dataContext.LeaveRequests.AddAsync(leaveRequest);
        }

        public async Task<LeaveRequest> GetLeaveRequestAsync(int LeaveRequestId)
        {
            return await _dataContext.LeaveRequests.FindAsync(LeaveRequestId);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsAsync(int UserId)
        {
            return await _dataContext.LeaveRequests
                .Where(x => x.LeaveSubmitterId == UserId)
                .ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

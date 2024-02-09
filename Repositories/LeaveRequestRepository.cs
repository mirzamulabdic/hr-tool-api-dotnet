using API.Data;
using API.DTOs;
using API.Entities;
using API.Enums;
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

        public void CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto)
        {
            var leaveRequest = new LeaveRequest
            {
                LeaveType = newLeaveRequestDto.LeaveType,
                StartDate = newLeaveRequestDto.StartDate,
                EndDate = newLeaveRequestDto.EndDate,
                DurationDays = newLeaveRequestDto.DurationDays,
                Comment = newLeaveRequestDto.Comment,
                LeaveStatus = LeaveStatusEnum.Pending,
                LeaveSubmitterId = newLeaveRequestDto.UserId,
                LeaveReviewerId = newLeaveRequestDto.ReviewerId,
                SubmitterFullName = newLeaveRequestDto.SubmitterFullName,
            };

            _dataContext.LeaveRequests.Add(leaveRequest);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetLeaveEventsToday()
        {

            var currentDate = DateTime.Today.Date;
            var leaveEvents = await _dataContext.LeaveRequests
                            .Where(lr => DateTime.UtcNow.Date >= lr.StartDate.Date && 
                                      (DateTime.UtcNow.Date <= lr.EndDate.Date || lr.StartDate.Date == lr.EndDate.Date)
                                  && lr.LeaveStatus == LeaveStatusEnum.Approved
                           )
                            .ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();

            return leaveEvents;
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

        public async Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsForMyEmployeesAsync(int ManagerId)
        {
            return await _dataContext.LeaveRequests
                .Where(x => x.LeaveReviewerId == ManagerId && x.LeaveStatus == LeaveStatusEnum.Pending)
                .ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void ReviewLeaveRequest(int leaveRequestId, LeaveStatusEnum leaveStatus)
        {
            var leaveRequest = _dataContext.LeaveRequests.SingleOrDefault(x => x.LeaveRequestId == leaveRequestId);
            
            if (leaveRequest == null) return;
            Console.WriteLine(leaveStatus);
            leaveRequest.LeaveStatus = leaveStatus;
            Console.WriteLine(leaveRequest.LeaveStatus);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

using API.Data;
using API.DTOs;
using API.Entities;
using API.Enums;
using API.Helpers;
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
                StartDate = newLeaveRequestDto.StartDate.ToUniversalTime(),
                EndDate = newLeaveRequestDto.EndDate.ToUniversalTime(),
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

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.ToLocalTime());

            var leaveEvents = await _dataContext.LeaveRequests
                                        .Where(lr => currentDate >= DateOnly.FromDateTime(lr.StartDate.ToLocalTime())
                                            && currentDate <= DateOnly.FromDateTime(lr.EndDate.ToLocalTime())
                                            && lr.LeaveStatus == LeaveStatusEnum.Approved)
                                    .ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync();

            return leaveEvents;
        }

        public async Task<LeaveRequest> GetLeaveRequestAsync(int LeaveRequestId)
        {
            return await _dataContext.LeaveRequests.FindAsync(LeaveRequestId);
        }

        public async Task<PagedList<LeaveRequestDto>> GetMyLeaveRequestsAsync(UserParams userParams)
        {
            var query = _dataContext.LeaveRequests.AsQueryable();

            query = query.Where(lr => lr.LeaveSubmitterId == userParams.CurrentUserId);
            query = query.Where(lr => lr.LeaveStatus == userParams.LeaveStatus);
            query = query.OrderByDescending(lr => lr.DateCreated);

            return await PagedList<LeaveRequestDto>.CreateAsync(
                        query.ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                        userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllMyLeaveRequestsWithoutPagiantionAsync(int EmployeeId)
        {
            return await _dataContext.LeaveRequests
                .Where(lr => lr.LeaveSubmitterId == EmployeeId && lr.LeaveStatus != LeaveStatusEnum.Rejected)
                .ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PagedList<LeaveRequestDto>> GetLeaveRequestsForMyEmployeesAsync(int ManagerId, UserParams userParams)
        {
            var query = _dataContext.LeaveRequests.AsQueryable();

            query = query.Where(lr => lr.LeaveReviewerId == ManagerId);

            if (userParams.LeaveStatus == LeaveStatusEnum.Approved) 
            {
                query = query.Where(lr => lr.LeaveStatus != LeaveStatusEnum.Rejected && lr.LeaveStatus != LeaveStatusEnum.Pending);
            } else
            {
                query = query.Where(lr => lr.LeaveStatus == LeaveStatusEnum.Pending);
            }

            query = query.OrderByDescending(lr => lr.DateCreated);


            return await PagedList<LeaveRequestDto>.CreateAsync(
                        query.ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                        userParams.PageNumber, userParams.PageSize);
        }

        public void ReviewLeaveRequest(int leaveRequestId, LeaveStatusEnum leaveStatus)
        {
            var leaveRequest = _dataContext.LeaveRequests.SingleOrDefault(x => x.LeaveRequestId == leaveRequestId);
            
            if (leaveRequest == null) return;

            leaveRequest.LeaveStatus = leaveStatus;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateTakenLeaveRequests()
        {

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.ToLocalTime());
            var leaveRequests = await _dataContext.LeaveRequests
                    .Where(lr=> lr.LeaveStatus == LeaveStatusEnum.Approved
                            && DateOnly.FromDateTime(lr.EndDate.ToLocalTime()) < currentDate)
                    .ToListAsync();

            foreach (var leaveRequest in leaveRequests)
            {
                leaveRequest.LeaveStatus = LeaveStatusEnum.Taken;

                _dataContext.Entry(leaveRequest).State = EntityState.Modified;
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}

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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public EmployeeRepository(DataContext dataContext, IMapper mapper)
        {
            this._dataContext = dataContext;
            this._mapper = mapper;
        }

        public Task<ActionResult> AddNewEmployee(NewEmployeeDto newEmployeeDto)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int Id)
        {
            return await _dataContext.Users
                .Where(x => x.Id == Id)
                .Include(lb=> lb.LeaveBalance)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<LeaveBalanceDto> GetLeaveBalance(int EmployeeID)
        {
            return await _dataContext.LeaveBalances
                .Where(x => x.EmployeeId == EmployeeID)
                .ProjectTo<LeaveBalanceDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

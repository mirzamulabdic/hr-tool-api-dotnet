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

        public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(int Id)
        {
            return await _dataContext.Users
                .Where(x => x.Id == Id)
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

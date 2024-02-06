using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly DataContext dataContext;

        public ManagerRepository(UserManager<AppUser> userManager, IMapper mapper, DataContext dataContext)
        {
            this._userManager = userManager;
            this._mapper = mapper;
            this.dataContext = dataContext;
        }

        public async Task<IEnumerable<EmployeesWithRolesDto>> GetEmployeesWithRoles()
        {
            var users = await dataContext.Users
                    .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                    .Select(u => new
                    {
                        Roles = u.UserRoles.Select(r => r.Role).ToList()
                    })
                    .ProjectTo<EmployeesWithRolesDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

            return users;
        }
    }
}

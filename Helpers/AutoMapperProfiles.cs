using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, EmployeeDto>()
                .ForMember(dest => dest.VacationDays, opt => opt.MapFrom(src => src.LeaveBalance.VacationDays))
                .ForMember(dest => dest.VacationDaysTaken, opt => opt.MapFrom(src => src.LeaveBalance.VacationDaysTaken))
                .ForMember(dest => dest.RemoteWorkDays, opt => opt.MapFrom(src => src.LeaveBalance.RemoteWorkDays))
                .ForMember(dest => dest.RemoteWorkDaysTaken, opt => opt.MapFrom(src => src.LeaveBalance.RemoteWorkDaysTaken))
                .ForMember(dest => dest.SickDays, opt => opt.MapFrom(src => src.LeaveBalance.SickDays))
                .ForMember(dest => dest.SickDaysTaken, opt => opt.MapFrom(src => src.LeaveBalance.SickDaysTaken))
                .ForMember(dest => dest.FamilyDays, opt => opt.MapFrom(src => src.LeaveBalance.FamilyDays))
                .ForMember(dest => dest.FamilyDaysTaken, opt => opt.MapFrom(src => src.LeaveBalance.FamilyDaysTaken))
                .ForMember(dest => dest.LeaveBalanceId, opt => opt.MapFrom(src => src.LeaveBalance.Id))
                .ForMember(dest => dest.ManagedBy, opt => opt.MapFrom(src => src.Manager.ManagerId));
            CreateMap<AppUser, EmployeesWithRolesDto>()
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.Manager.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)));

            CreateMap<UserManage, UserManagingDto>();
            CreateMap<LeaveBalance, LeaveBalanceDto>();
            CreateMap<LeaveRequest, LeaveRequestDto>();




        }
    }
}

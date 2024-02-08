using API.Data;
using API.DTOs;
using API.Entities;
using API.Enums;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Repositories
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly DataContext _dataContext;

        public LeaveBalanceRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<LeaveBalance> UpdateLeaveBalance(int leaveBalanceId, LeaveTypeEnum leaveType, int days)
        {

            var result =  _dataContext.LeaveBalances.SingleOrDefault(x=> x.Id == leaveBalanceId);

            if (result == null) return null;

            switch(leaveType)
            {
                case LeaveTypeEnum.Vacation:
                    result.VacationDaysTaken += days;
                    break;
                case LeaveTypeEnum.RemoteWork:
                    result.RemoteWorkDaysTaken += days;
                    break;
                case LeaveTypeEnum.SickDay:
                    result.SickDaysTaken += days;
                    break;
                case LeaveTypeEnum.FamilyLeave:
                    result.FamilyDaysTaken += days;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}

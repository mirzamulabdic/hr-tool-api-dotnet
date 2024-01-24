using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        public void UpdateLeaveBalance(int leaveBalanceId, string leaveType, int days)
        {

            var result = _dataContext.LeaveBalances.SingleOrDefault(x=> x.Id == leaveBalanceId);
            
            if (result != null)

            switch(leaveType)
            {
                case "vacation":
                    result.VacationDaysTaken += days;
                    break;
                case "remotework":
                    result.RemoteWorkDaysTaken += days;
                    break;
                case "sickday":
                    result.SickDaysTaken += days;
                    break;
                case "familyleave":
                    result.FamilyDaysTaken += days;
                    break;
                default:
                    break;
            }
        }
    }
}

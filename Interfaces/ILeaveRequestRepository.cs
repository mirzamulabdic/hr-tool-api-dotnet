using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<bool> SaveAllAsync();
        void CreateLeaveRequest(NewLeaveRequestDto newLeaveRequestDto);
    }
}

using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LeaveRequestController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public LeaveRequestController(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        [HttpPost]
        public async Task<ActionResult> CreteLeaveRequest(LeaveRequestDto leaveRequestDto)
        {
       

            return BadRequest();
        }
    }
}

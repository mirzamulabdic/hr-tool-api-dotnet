using API.Enums;

namespace API.DTOs
{
    public class LeaveBalanceUpdateDto
    {
        public int LeaveBalanceId { get; set; }
        public LeaveTypeEnum LeaveType { get; set; }
        public int Days { get; set; }
    }
}

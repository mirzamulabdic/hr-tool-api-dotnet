using API.Enums;

namespace API.DTOs
{
    public class ReviewLeaveRequestDto
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public int DurationDays { get; set; }
        public LeaveTypeEnum LeaveType { get; set; }
        public LeaveStatusEnum LeaveStatusAction { get; set; }

    }
}

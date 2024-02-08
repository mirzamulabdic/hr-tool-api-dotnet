using API.Enums;

namespace API.DTOs
{
    public class NewLeaveRequestDto
    {
        public int UserId { get; set; }
        public int ReviewerId { get; set; }
        public string SubmitterFullName { get; set; }
        public LeaveTypeEnum LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string Comment { get; set; }
    }
}

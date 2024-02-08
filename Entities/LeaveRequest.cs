using API.Enums;

namespace API.Entities
{


    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }
        public string SubmitterFullName { get; set; }
        public LeaveStatusEnum LeaveStatus { get; set; }
        public LeaveTypeEnum LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public int DurationDays { get; set; }
        public string Comment { get; set; }
        public int LeaveSubmitterId { get; set; }
        public AppUser LeaveSubmitter { get; set; }
        public int LeaveReviewerId { get; set; }
        public AppUser LeaveReviewer { get; set; }

    }
}

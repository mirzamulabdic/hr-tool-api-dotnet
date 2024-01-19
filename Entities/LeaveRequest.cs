namespace API.Entities
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string Comment { get; set; }
        public int LeaveSubmitterId { get; set; }
        public AppUser LeaveSubmitter { get; set; }
        public int LeaveReviewerId { get; set; }
        public AppUser LeaveReviewer { get; set; }

    }
}

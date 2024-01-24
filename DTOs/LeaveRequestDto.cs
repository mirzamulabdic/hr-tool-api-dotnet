namespace API.DTOs
{
    public class LeaveRequestDto
    {
        public int LeaveRequestId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string Comment { get; set; }
    }
}

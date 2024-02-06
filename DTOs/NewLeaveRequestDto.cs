namespace API.DTOs
{
    public class NewLeaveRequestDto
    {
        public int UserId { get; set; }
        public int ReviewerId { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string Comment { get; set; }
    }
}

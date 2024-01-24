namespace API.DTOs
{
    public class LeaveBalanceUpdateDto
    {
        public int LeaveBalanceId { get; set; }
        public string LeaveType { get; set; }
        public int Days { get; set; }
    }
}


namespace API.DTOs
{
    public class LeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        public int VacationDays { get; set; } = 20;
        public int VacationDaysTaken { get; set; } = 0;
        public int RemoteWorkDays { get; set; } = 20;
        public int RemoteWorkDaysTaken { get; set; } = 0;
        public int SickDays { get; set; } = 3;
        public int SickDaysTaken { get; set; } = 0;
        public int FamilyDays { get; set; } = 7;
        public int FamilyDaysTaken { get; set; } = 0;
    }
}

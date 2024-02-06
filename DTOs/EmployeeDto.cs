using API.Entities;

namespace API.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly JoinedDate { get; set; }
        public int VacationDays { get; set; }
        public int VacationDaysTaken { get; set; }
        public int RemoteWorkDays { get; set; }
        public int RemoteWorkDaysTaken { get; set; }
        public int SickDays { get; set; }
        public int SickDaysTaken { get; set; }
        public int FamilyDays { get; set; }
        public int FamilyDaysTaken { get; set; }
        public int LeaveBalanceId { get; set; }
        public int? ManagedBy { get; set; } = null;
        public List<UserManagingDto> ManagedEmployees { get; set; }
    }
}

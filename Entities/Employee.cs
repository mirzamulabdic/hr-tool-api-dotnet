using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Employee
    {
        [ForeignKey("AppUser")]
        public int EmployeeId { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly JoinedDate { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        //public Location Location { get; set; }
        //public int LocationId { get; set; }
        //public LeaveBalance LeaveBalance { get; set; }
        //public int LeaveBalanceId { get; set; }
    }
}

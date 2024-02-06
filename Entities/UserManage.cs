namespace API.Entities
{
    public class UserManage
    {
        public int Id { get; set; }
        public AppUser Manager { get; set; }
        public int ManagerId { get; set; }
        public AppUser Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}

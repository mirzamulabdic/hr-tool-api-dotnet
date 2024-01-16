namespace API.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }
        public string OfficeName { get; set; }
        public List<Employee> Employees { get; set; }
    }
}

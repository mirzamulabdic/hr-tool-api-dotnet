﻿namespace API.DTOs
{
    public class LeaveRequestDto
    {
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
    }
}

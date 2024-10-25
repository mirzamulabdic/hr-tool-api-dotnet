﻿using API.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public DateTime BirthDate { get; set; }
        public DateOnly JoinedDate { get; set; }
        public List<LeaveRequest> LeaveRequestCreated { get; set; }
        public List<LeaveRequest> LeaveRequestReviewed { get; set; }
        public LeaveBalance LeaveBalance { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public UserManage Manager { get; set; }
        public ICollection<UserManage> ManagedEmployees { get; set; }

    }
}

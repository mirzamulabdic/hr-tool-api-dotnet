using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<UserManage> UserManages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
            .Property(d => d.Gender)
            .HasConversion<string>();

            modelBuilder.Entity<LeaveRequest>()
            .Property(d => d.LeaveStatus)
            .HasConversion<string>();

            modelBuilder.Entity<LeaveRequest>()
            .Property(d => d.LeaveType)
            .HasConversion<string>();

            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<UserManage>()
                .HasOne(um => um.Manager)
                .WithMany(u => u.ManagedEmployees)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserManage>()
                .HasOne(um => um.Employee)
                .WithOne(u => u.Manager)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(ls => ls.LeaveSubmitter)
                .WithMany(c => c.LeaveRequestCreated)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.LeaveReviewer)
                .WithMany(r => r.LeaveRequestReviewed)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasOne(e => e.LeaveBalance)
                .WithOne(lb => lb.Employee)
                .HasForeignKey<LeaveBalance>(lb => lb.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

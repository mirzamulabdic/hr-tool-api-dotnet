using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

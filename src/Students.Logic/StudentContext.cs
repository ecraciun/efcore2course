using Microsoft.EntityFrameworkCore;

namespace Students.Logic
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Disenrollment> Disenrollments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Disenrollment>().HasOne(x => x.Student).WithMany(x => x.Disenrollments).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Disenrollment>().HasOne(x => x.Course);

            modelBuilder.Entity<Enrollment>().HasOne(x => x.Student).WithMany(x => x.Enrollments).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Enrollment>().HasOne(x => x.Course);
        }
    }
}
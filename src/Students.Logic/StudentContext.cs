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
            modelBuilder.Entity<Student>().HasMany(x => x.Disenrollments).WithOne(x => x.Student).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Student>().HasMany(x => x.Enrollments).WithOne(x => x.Student).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Disenrollment>().HasOne(x => x.Course);            
            modelBuilder.Entity<Enrollment>().HasOne(x => x.Course);
        }
    }
}
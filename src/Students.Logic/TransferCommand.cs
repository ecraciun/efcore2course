using System;

namespace Students.Logic
{
    public sealed class TransferCommand : ICommand
    {
        public long Id { get; }
        public int EnrollmentNumber { get; }
        public long CourseId { get; }
        public Grade Grade { get; }

        public TransferCommand(long id, int enrollmentNumber, long courseId, Grade grade)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            CourseId = courseId;
            Grade = grade;
        }
    }
}
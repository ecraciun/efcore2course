using System;

namespace Students.Logic
{
    public sealed class EnrollCommand : ICommand
    {
        public long Id { get; }
        public long CourseId { get; }
        public Grade Grade { get; }

        public EnrollCommand(long id, long courseId, Grade grade)
        {
            Id = id;
            CourseId = courseId;
            Grade = grade;
        }
    }
}
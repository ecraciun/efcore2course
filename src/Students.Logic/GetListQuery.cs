using System.Collections.Generic;

namespace Students.Logic
{
    public sealed class GetListQuery : IQuery<List<StudentDto>>
    {
        public GetListQuery(string enrolledIn, int? numberOfCourses)
        {
            EnrolledIn = enrolledIn;
            NumberOfCourses = numberOfCourses;
        }

        public string EnrolledIn { get; }
        public int? NumberOfCourses { get; }
    }
}
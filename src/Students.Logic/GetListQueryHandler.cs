using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Students.Logic
{
    public sealed class GetListQueryHandler : IQueryHandler<GetListQuery, List<StudentDto>>
    {
        private readonly ConnectionString _connectionString;
        public GetListQueryHandler(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public List<StudentDto> Handle(GetListQuery query)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                string sql = @"
                    SELECT s.*, e.Grade, c.Name CourseName, c.Credits
                    FROM dbo.Students s
                    LEFT JOIN (
                        SELECT e.StudentId, COUNT(*) Number
                        FROM dbo.Enrollments e
                        GROUP BY e.StudentId) t ON s.Id = t.StudentId
                    LEFT JOIN dbo.Enrollments e ON s.Id = e.StudentId
                    LEFT JOIN dbo.Courses c ON e.CourseId = c.Id
                    WHERE (c.Name = @Course OR @Course IS NULL) AND
                            (ISNULL(t.Number, 0) = @Number OR @Number IS NULL)
                    ORDER BY s.Id ASC";

                List<StudentInDB> students = connection.Query<StudentInDB>(sql, new
                {
                    Course = query.EnrolledIn,
                    Number = query.NumberOfCourses
                }).ToList();

                List<long> ids = students.GroupBy(x => x.Id).Select(x => x.Key).ToList();

                var result = new List<StudentDto>();

                foreach(var id in ids)
                {
                    var data = students.Where(x => x.Id == id).ToList();

                    var dto = new StudentDto
                    {
                        Id = data[0].Id,
                        Name = data[0].Name,
                        Email = data[0].Email,
                        Course1 = data[0].CourseName,
                        Course1Credits = data[0].Credits,
                        Course1Grade = data[0]?.Grade.ToString()
                    };
                    if(data.Count > 1)
                    {
                        dto.Course2 = data[1].CourseName;
                        dto.Course2Credits = data[1].Credits;
                        dto.Course2Grade = data[1]?.Grade.ToString();
                    }

                    result.Add(dto);
                }

                return result;
            }
        }

        private class StudentInDB
        {
            public readonly long Id;
            public readonly string Name;
            public readonly string Email;
            public readonly Grade? Grade;
            public readonly string CourseName;
            public readonly int? Credits;

            public StudentInDB(long id, string name, string email, Grade? grade, string courseName, int? credits)
            {
                Id = id;
                Name = name;
                Email = email;
                Grade = grade;
                CourseName = courseName;
                Credits = credits;
            }


        }
    }
}
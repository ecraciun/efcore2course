using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students.Logic
{
    public sealed class GetListQueryHandler : IQueryHandler<GetListQuery, List<StudentDto>>
    {
        private readonly StudentRepository _studentRepository;
        public GetListQueryHandler(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public List<StudentDto> Handle(GetListQuery query)
        {
            var students = _studentRepository.GetList(query.EnrolledIn, query.NumberOfCourses)
                .Select(x => ConvertToDto(x)).ToList();
            return students;
        }

        private StudentDto ConvertToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Course1 = student.FirstEnrollment?.Course?.Name,
                Course1Grade = student.FirstEnrollment?.Grade.ToString(),
                Course1Credits = student.FirstEnrollment?.Course?.Credits,
                Course2 = student.SecondEnrollment?.Course?.Name,
                Course2Grade = student.SecondEnrollment?.Grade.ToString(),
                Course2Credits = student.SecondEnrollment?.Course?.Credits,
            };
        }
    }
}

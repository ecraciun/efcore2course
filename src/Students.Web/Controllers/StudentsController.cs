using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Students.Logic;
using Students.Web.Dtos;

namespace Students.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentsController(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        [HttpGet]
        [Route("[controller]/[action]/{enrolled?}/{number?}")]
        public IActionResult Index(string enrolled, int? number)
        {
            return View(
                _studentRepository.GetList(enrolled, number).Select(x => ConvertToDto(x)).ToList()
                );
        }

        public IActionResult Register()
        {
            var courses = _courseRepository.GetAll();
            courses.Insert(0, new Course());
            var createVM = new CreateStudentViewModel
            {
                Courses = courses.Select(x => new SelectListItem(x.ToString(), x.Id.ToString())).ToList(),
                Grades = Enum.GetValues(typeof(Grade)).Cast<Grade>()
                    .Select(x => new SelectListItem(Enum.GetName(typeof(Grade), x), x.ToString())).ToList()
            };
            return View(createVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(
            [FromForm]CreateStudentDto student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.Add(ConvertFromCreateVmToStudent(student));
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public IActionResult Edit(long id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(new StudentPersonalInfoDto
            {
                Email = student.Email,
                Name = student.Name
            });
        }

        public IActionResult Unregister(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _studentRepository.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(ConvertToDto(student));
        }

        [HttpPost, ActionName("Unregister")]
        [ValidateAntiForgeryToken]
        public IActionResult UnregisterConfirmed(long id)
        {
            var student = _studentRepository.GetById(id);
            _studentRepository.Delete(student);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[controller]/{id}/enrollments")]
        public IActionResult Enroll(long id, [FromForm]StudentEnrollmentDto dto)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }

            var course = _courseRepository.GetById(dto.CourseId);
            if(course == null)
            {
                return NotFound();
            }

            student.Enroll(course, dto.Grade);
            _studentRepository.Update(student);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("[controller]/{id}/enrollments")]
        public IActionResult Enroll(long id)
        {
            return View("Enroll");
        }

        [HttpGet("[controller]/{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber)
        {
            return View("Transfer");
        }

        [HttpGet("[controller]/{id}/enrollments/{enrollmentNumber}/delete")]
        public IActionResult Disenroll(long id, int enrollmentNumber)
        {
            return View("Disenroll");
        }

        [HttpPost("[controller]/{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromForm]StudentTransferDto dto)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }

            var course = _courseRepository.GetById(dto.CourseId);
            if (course == null)
            {
                return NotFound();
            }

            var enrollment = student.GetEnrollment(enrollmentNumber);
            if(enrollment == null)
            {
                return NotFound();
            }

            enrollment.Update(course, dto.Grade);
            _studentRepository.Update(student);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[controller]/{id}/enrollments/{enrollmentNumber}/delete")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromForm]StudentDisenrollmentDto dto)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(dto.Comment))
            {
                return NotFound();
            }
            var enrollment = student.GetEnrollment(enrollmentNumber);
            if (enrollment == null)
            {
                return NotFound();
            }

            //student.RemoveEnrollment(enrollment);
            //student.AddDisenrollmentComment(enrollment, studentDisenrollmentDto.Comment);

            student.RemoveEnrollment(enrollment, dto.Comment);
            _studentRepository.Update(student);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[controller]/{id}")]
        public IActionResult UpdatePersonalInfo(long id, [FromForm]StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand
            {
                Email = dto.Email,
                Name = dto.Name,
                Id = id
            };
            var handler = new EditPersonalInfoCommandHandler(_studentRepository);
            var result = handler.Handle(command);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
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

        private EditStudentVm ConvertToEditVm(Student student)
        {
            EditStudentVm result = new EditStudentVm
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };

            if (student.FirstEnrollment != null)
            {
                result.Course1Id = student.FirstEnrollment.Course.Id;
                result.Course1Grade = Enum.GetName(typeof(Grade), student.FirstEnrollment.Grade);
            }
            if (student.SecondEnrollment != null)
            {
                result.Course2Id = student.SecondEnrollment.Course.Id;
                result.Course2Grade = Enum.GetName(typeof(Grade), student.SecondEnrollment.Grade);
            }

            var allCourses = _courseRepository.GetAll();
            allCourses.Insert(0, new Course());
            result.CoursesForC1 = allCourses.Select(
                x => new SelectListItem(x.ToString(), x.Id.ToString(), x.Id == result.Course1Id)).ToList();
            result.CoursesForC2 = allCourses.Select(
                x => new SelectListItem(x.ToString(), x.Id.ToString(), x.Id == result.Course2Id)).ToList();

            result.GradesForC1 = Enum.GetValues(typeof(Grade)).Cast<Grade>()
                    .Select(x => new SelectListItem(Enum.GetName(typeof(Grade), x), x.ToString(),
                    x == student.FirstEnrollment?.Grade)).ToList();
            result.GradesForC2 = Enum.GetValues(typeof(Grade)).Cast<Grade>()
                    .Select(x => new SelectListItem(Enum.GetName(typeof(Grade), x), x.ToString(),
                    x == student.SecondEnrollment?.Grade)).ToList();

            return result;
        }

        private Student ConvertFromCreateVmToStudent(CreateStudentDto studentVm)
        {
            Student result = null;

            if (studentVm != null)
            {
                result = new Student(studentVm.Name, studentVm.Email);
                if (studentVm.Course1Id != default)
                {

                    result.Enrollments.Add(new Enrollment(
                        result,
                        _courseRepository.GetById(studentVm.Course1Id),
                         (Grade)Enum.Parse(typeof(Grade), studentVm.Course1Grade)));
                }

                if (studentVm.Course2Id != default)
                {
                    result.Enrollments.Add(new Enrollment(
                        result,
                        _courseRepository.GetById(studentVm.Course2Id),
                         (Grade)Enum.Parse(typeof(Grade), studentVm.Course2Grade)));
                }
            }

            return result;
        }

        private Grade GetGradeFromString(string grade)
        {
            return (Grade)Enum.Parse(typeof(Grade), grade);
        }
    }
}

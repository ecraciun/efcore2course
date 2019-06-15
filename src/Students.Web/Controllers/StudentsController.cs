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

        public IActionResult Create()
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

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("Name,Email,Course1Id,Course1Grade,Course2Id,Course2Grade")] CreateStudentViewModel student)
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
            return View(ConvertToEditVm(student));
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            [Bind("Id,Name,Email,Course1Id,Course1Grade,Course2Id,Course2Grade,C1DisenrollmentComment,C2DisenrollmentComment")] EditStudentVm studentVm)
        {
            var student = ConvertFromEditVmToStudent(studentVm);
            if (student == null) return new RedirectToActionResult(nameof(Edit), "Students", studentVm.Id);

            _studentRepository.Update(student);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(long? id)
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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var student = _studentRepository.GetById(id);
            _studentRepository.Delete(student);
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

        private Student ConvertFromCreateVmToStudent(CreateStudentViewModel studentVm)
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

        private Student ConvertFromEditVmToStudent(EditStudentVm studentVm)
        {
            var student = _studentRepository.GetById(studentVm.Id);
            if (student == null) return null;

            // If disenrolled -> must provide reason
            if (studentVm.Course2Id == default && student.SecondEnrollment != null &&
                string.IsNullOrEmpty(studentVm.C2DisenrollmentComment))
                return null;
            if (studentVm.Course1Id == default && student.FirstEnrollment != null &&
                string.IsNullOrEmpty(studentVm.C1DisenrollmentComment))
                return null;


            // No initial first enrollment
            if (student.SecondEnrollment == null && studentVm.Course2Id != default)
            {
                student.Enroll(_courseRepository.GetById(studentVm.Course2Id),
                    GetGradeFromString(studentVm.Course2Grade));
            }
            if (student.FirstEnrollment == null && studentVm.Course1Id != default)
            {
                student.Enroll(_courseRepository.GetById(studentVm.Course1Id),
                    GetGradeFromString(studentVm.Course1Grade));
            }

            // Disenrolled
            if (student.SecondEnrollment != null && studentVm.Course2Id == default)
            {
                var enrollmentToRemove = student.SecondEnrollment;
                student.RemoveEnrollment(enrollmentToRemove);
                student.AddDisenrollmentComment(enrollmentToRemove, studentVm.C2DisenrollmentComment);
            }
            if (student.FirstEnrollment != null && studentVm.Course1Id == default)
            {
                var enrollmentToRemove = student.FirstEnrollment;
                student.RemoveEnrollment(enrollmentToRemove);
                student.AddDisenrollmentComment(enrollmentToRemove, studentVm.C1DisenrollmentComment);
            }

            // Changed enrollement
            if (student.SecondEnrollment != null && studentVm.Course2Id != student.SecondEnrollment.Course.Id)
            {
                student.RemoveEnrollment(student.SecondEnrollment);
                student.Enroll(_courseRepository.GetById(studentVm.Course2Id),
                    GetGradeFromString(studentVm.Course2Grade));
            }
            if (student.FirstEnrollment != null && studentVm.Course2Id != student.FirstEnrollment.Course.Id)
            {
                student.RemoveEnrollment(student.FirstEnrollment);
                student.Enroll(_courseRepository.GetById(studentVm.Course1Id),
                    GetGradeFromString(studentVm.Course1Grade));
            }

            // Changed grade
            if (student.SecondEnrollment != null && student.SecondEnrollment.Course.Id == studentVm.Course2Id)
            {
                student.SecondEnrollment.Grade = GetGradeFromString(studentVm.Course2Grade);
            }
            if (student.FirstEnrollment != null && student.FirstEnrollment.Course.Id == studentVm.Course1Id)
            {
                student.FirstEnrollment.Grade = GetGradeFromString(studentVm.Course1Grade);
            }

            student.Name = studentVm.Name;
            student.Email = studentVm.Email;

            return student;
        }

        private Grade GetGradeFromString(string grade)
        {
            return (Grade)Enum.Parse(typeof(Grade), grade);
        }
    }
}

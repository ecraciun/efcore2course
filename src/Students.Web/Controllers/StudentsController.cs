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
        private readonly Messages _messages;

        public StudentsController(StudentRepository studentRepository, CourseRepository courseRepository, Messages messages)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _messages = messages;
        }

        [HttpGet]
        [Route("[controller]/[action]/{enrolled?}/{number?}")]
        public IActionResult Index(string enrolled, int? number)
        {
            return View(_messages.Dispatch(new GetListQuery(enrolled, number)));
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
            [FromForm]CreateStudentDto dto)
        {
            var command = new RegisterCommand(dto.Name, dto.Email, dto.Course1Id, dto.Course1Grade, dto.Course2Id, dto.Course2Grade);
            var result = _messages.Dispatch(command);
            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
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
                Name = student.Name,
                Id = student.Id
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
            var command = new UnregisterCommand(id);
            var result = _messages.Dispatch(command);

            if (!result.IsSuccess)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[controller]/{id}/enrollments")]
        public IActionResult Enroll(long id, [FromForm]StudentEnrollmentDto dto)
        {
            var command = new EnrollCommand(id, dto.CourseId, dto.Grade);
            var result = _messages.Dispatch(command);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

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
            var command = new TransferCommand(id, enrollmentNumber, dto.CourseId, dto.Grade);
            var result = _messages.Dispatch(command);

            if (!result.IsSuccess)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[controller]/{id}/enrollments/{enrollmentNumber}/delete")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromForm]StudentDisenrollmentDto dto)
        {
            var command = new DisenrollCommand(id, enrollmentNumber, dto.Comment);
            var result = _messages.Dispatch(command);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("[controller]/{id}")]
        public IActionResult UpdatePersonalInfo(long id, [FromForm]StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Name, dto.Email);
            var result = _messages.Dispatch(command);

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
                Course1 = student.GetEnrollment(0)?.Course?.Name,
                Course1Grade = student.GetEnrollment(0)?.Grade.ToString(),
                Course1Credits = student.GetEnrollment(0)?.Course?.Credits,
                Course2 = student.GetEnrollment(1)?.Course?.Name,
                Course2Grade = student.GetEnrollment(1)?.Grade.ToString(),
                Course2Credits = student.GetEnrollment(1)?.Course?.Credits,
            };
        }
    }
}
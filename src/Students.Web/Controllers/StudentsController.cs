using System;
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
        public async Task<IActionResult> Index(string enrolled, int? number)
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
                    .Select(x => new SelectListItem(Enum.GetName(typeof(Grade),x), x.ToString())).ToList()
            };
            return View(createVM);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Email,Course1Id,Course1Grade,Course2Id,Course2Grade")] CreateStudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                _studentRepository.Add(ConvertFromCreateStudentDtoToStudent(student));
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        private Student ConvertFromCreateStudentDtoToStudent(CreateStudentViewModel studentDto)
        {
            Student result = null;

            if(studentDto != null)
            {
                result = new Student(studentDto.Name, studentDto.Email);
                if(studentDto.Course1Id != default)
                {

                    result.Enrollments.Add(new Enrollment(
                        result,
                        _courseRepository.GetById(studentDto.Course1Id),
                         (Grade)Enum.Parse(typeof(Grade), studentDto.Course1Grade)));
                }

                if (studentDto.Course2Id != default)
                {
                    result.Enrollments.Add(new Enrollment(
                        result,
                        _courseRepository.GetById(studentDto.Course2Id),
                         (Grade)Enum.Parse(typeof(Grade), studentDto.Course2Grade)));
                }
            }

            return result;
        }

        //// GET: Students/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await _context.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(student);
        //}

        //// POST: Students/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("Name,Email,Id")] Student student)
        //{
        //    if (id != student.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(student);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!StudentExists(student.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(student);
        //}


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
    }
}

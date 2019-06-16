using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Students.Web.Dtos
{
    public class CreateStudentViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public List<SelectListItem> Courses { get; set; }
        public List<SelectListItem> Grades { get; set; }

        public long Course1Id { get; set; }
        public string Course1Grade { get; set; }

        public long Course2Id { get; set; }
        public string Course2Grade { get; set; }
    }
}
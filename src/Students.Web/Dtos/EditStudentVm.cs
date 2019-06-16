using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Students.Web.Dtos
{
    public class EditStudentVm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<SelectListItem> CoursesForC1 { get; set; }
        public List<SelectListItem> GradesForC1 { get; set; }

        public List<SelectListItem> CoursesForC2 { get; set; }
        public List<SelectListItem> GradesForC2 { get; set; }

        public long Course1Id { get; set; }
        public string Course1Grade { get; set; }

        public long Course2Id { get; set; }
        public string Course2Grade { get; set; }

        public string C1DisenrollmentComment { get; set; }
        public string C2DisenrollmentComment { get; set; }
    }
}
namespace Students.Web.Dtos
{
    public class CreateStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public long Course1Id { get; set; }
        public string Course1Grade { get; set; }

        public long Course2Id { get; set; }
        public string Course2Grade { get; set; }
    }
}
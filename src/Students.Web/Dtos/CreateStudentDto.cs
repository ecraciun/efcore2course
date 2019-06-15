namespace Students.Web.Dtos
{
    public class CreateStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public long Course1Id { get; set; }
        public int? Course1Grade { get; set; }

        public long Course2Id { get; set; }
        public int? Course2Grade { get; set; }
    }
}
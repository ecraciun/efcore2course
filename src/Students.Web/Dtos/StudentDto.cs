﻿namespace Students.Web.Dtos
{
    public sealed class StudentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Course1 { get; set; }
        public long Course1Id { get; set; }
        public string Course1Grade { get; set; }
        public string Course1DisenrollmentComment { get; set; }
        public int? Course1Credits { get; set; }

        public long Course2Id { get; set; }
        public string Course2 { get; set; }
        public string Course2Grade { get; set; }
        public string Course2DisenrollmentComment { get; set; }
        public int? Course2Credits { get; set; }
    }
}
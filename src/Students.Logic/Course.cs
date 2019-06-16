namespace Students.Logic
{
    public class Course : Entity
    {
        public string Name { get; protected set; }
        public int Credits { get; protected set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name)) return null;
            return $"{Name} - {Credits} credits";
        }
    }
}
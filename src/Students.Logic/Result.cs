namespace Students.Logic
{
    public class Result
    {
        public static Result Success()
        {
            return new Result
            {
                IsSuccess = true
            };
        }

        public static Result Fail(string message)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message
            };
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
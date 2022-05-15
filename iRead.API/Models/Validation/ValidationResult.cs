namespace iRead.API.Models.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new Dictionary<string, string>();
        }

        public ValidationFailType? ValidationFailType { get; set; } = null;
        public bool Success { get; set; } = true;
        public Dictionary<string, string> Errors { get; set; }
        public IEnumerable<string> EmptyFields { get; set; }
    }

    public enum ValidationFailType
    {
        EmptyFields,
        WrongData
    }

}

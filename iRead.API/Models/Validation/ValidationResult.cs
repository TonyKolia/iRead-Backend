namespace iRead.API.Models.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Messages = new List<string>();
        }

        public bool Success { get; set; } = true;
        public List<string> Messages { get; set; }
    }
}

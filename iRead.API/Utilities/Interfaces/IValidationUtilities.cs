using iRead.API.Models.Account;
using iRead.API.Models.Validation;

namespace iRead.API.Utilities.Interfaces
{
    public interface IValidationUtilities
    {
        bool IsObjectCompletelyPopulated(object obj);
        IEnumerable<string> FindEmptyObjectFields(object obj);
        Task<ValidationResult> ValidateRegistrationForm(RegistrationForm form);
        Task<ValidationResult> ValidateOrder(IEnumerable<int> orderItems, int userId);
        Task<ValidationResult> ValidateEmail(string email);
        Task<ValidationResult> ValidatePasswordChange(int userId, string password, string confirmPassword);
    }
}

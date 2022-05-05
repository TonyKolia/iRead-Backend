using iRead.API.Models.Account;
using iRead.API.Models.Validation;

namespace iRead.API.Utilities.Interfaces
{
    public interface IValidationUtilities
    {
        bool IsObjectCompletelyPopulated(object obj);
        ValidationResult ValidateRegistrationForm(RegistrationForm form);
    }
}

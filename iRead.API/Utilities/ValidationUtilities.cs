using iRead.API.Models.Account;
using iRead.API.Models.Validation;
using iRead.API.Utilities.Interfaces;

namespace iRead.API.Utilities
{
    public class ValidationUtilities : IValidationUtilities
    {
        public bool IsObjectCompletelyPopulated(object obj)
        {
            foreach(var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value == null)
                    return false;

                if (prop.GetType() == typeof(string) && string.IsNullOrEmpty(value.ToString()))
                    return false;
            }

            return true;
        }

        public ValidationResult ValidateRegistrationForm(RegistrationForm form)
        {
            var res = new ValidationResult();

            return res;
        }
    }
}

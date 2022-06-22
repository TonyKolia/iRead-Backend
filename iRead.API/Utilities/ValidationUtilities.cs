using iRead.API.Models.Account;
using iRead.API.Models.Validation;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities.Interfaces;

namespace iRead.API.Utilities
{
    public class ValidationUtilities : IValidationUtilities
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IBookRepository _bookRepository;

        public ValidationUtilities(IUserRepository _userRepository, IMemberRepository _memberRepository, IBookRepository _bookRepository)
        {
            this._userRepository = _userRepository;
            this._memberRepository = _memberRepository;
            this._bookRepository = _bookRepository;
        }

        public bool IsObjectCompletelyPopulated(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value == null || value == "")
                    return false;
            }

            return true;
        }

        public IEnumerable<string> FindEmptyObjectFields(object obj)
        {
            var emptyFields = new List<string>();

            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value == null || value == "" ||  (prop.Name == "AcceptTerms" && value as bool? == false) )
                    emptyFields.Add(char.ToLower(prop.Name[0]) + prop.Name.Substring(1));
            }

            return emptyFields;
        }

        public async Task<ValidationResult> ValidateRegistrationForm(RegistrationForm form)
        {
            var res = new ValidationResult();

            res.EmptyFields = FindEmptyObjectFields(form);
            if(res.EmptyFields.Count() > 0)
            {
                res.Success = false;
                res.ValidationFailType = ValidationFailType.EmptyFields;
                return res;
            }

            if (await _userRepository.UserExists(form.Username))
                res.Errors.Add("username", "Το όνομα χρήστη υπάρχει ήδη.");

            if(form.Password != form.ConfirmPassword)
                res.Errors.Add("confirmPassword", "Οι κωδικοί πρόσβασης δεν ταιριάζουν.");

            if(form.Birthdate.Value.Year > DateTime.Now.Year || form.Birthdate.Value.Year < DateTime.Now.Year - 100)
                res.Errors.Add("birthdate", "Μη έγκυρη ημερομηνία γέννησης.");
            else if(DateTime.Now.Year - form.Birthdate.Value.Year < 12)
                res.Errors.Add("birthdate", "Η εγγραφή επιτρέπεται σε άτομα άνω των 12 ετών.");

            if(await _memberRepository.IdNumberExists(form.IdNumber))
                res.Errors.Add("idNumber", "Ο αριθμός εγγράφου ταυτοποίησης χρησιμοποιείται ήδη.");

            if(form.PostalCode.Length != 5)
                res.Errors.Add("postalCode", "Μη έγκυρος ταχυδρομικός κώδικας.");

            if(form.Telephone.Length < 10)
                res.Errors.Add("telephone", "Μη έγκυρος αριθμός τηλεφώνου.");

            if(!IsEmailValid(form.Email))
                res.Errors.Add("email", "Μη έγκυρη διεύθυνση email.");

            if (form.FavoriteCategories.Count() == 0)
                res.Errors.Add("favoriteCategories", "Παρακαλώ επιλέξτε τουλάχιστον μία κατηγορία προτίμησης.");

            if(form.FavoriteCategories.Count() > 3)
                res.Errors.Add("favoriteCategories", "Οι κατηγορίες προτίμησης δεν μπορούν να είναι περισσότερες από 3.");

            if (form.FavoriteAuthors.Count() == 0)
                res.Errors.Add("favoriteAuthors", "Παρακαλώ επιλέξτε τουλάχιστον έναν συγγραφέα προτίμησης.");

            if (form.FavoriteAuthors.Count() > 3)
                res.Errors.Add("favoriteAuthors", "Οι συγγραφείς προτίμησης δεν μπορούν να είναι περισσότεροι από 3.");

            if (res.Errors.Count() > 0)
            {
                res.Success = false;
                res.ValidationFailType = ValidationFailType.WrongData;
                return res;
            }

            return res;
        }

        private bool IsEmailValid(string email)
        {
            if (!email.Contains('@'))
                return false;

            if (email.Count(x => x == '@') > 1)
                return false;

            var emailParts = email.Split('@');
            if (emailParts.Length != 2 || emailParts[0].Length == 0 || emailParts[1].Length == 1)
                return false;

            if (!emailParts[1].Contains('.'))
                return false;

            if (emailParts[1].Split('.')[0].Length == 0)
                return false;

            return true;
        }

        public async Task<ValidationResult> ValidateOrder(IEnumerable<int> orderItems, int userId)
        {
            var res = new ValidationResult();

            if (!await _userRepository.UserExists(userId))
                res.Errors.Add("user", "Ο χρήστης δεν υπάρχει.");

            if(!await _userRepository.UserActive(userId))
                res.Errors.Add("userNotActive", "Ο λογαριασμός σας δεν έχει ενεργοποιηθεί.");

            if (orderItems.Count() == 0)
                res.Errors.Add("noItems", "Δεν προστέθηκαν βιβλία στην παραγγελία.");

            if(res.Errors.Count() > 0)
            {
                res.Success = false;
                res.ValidationFailType = ValidationFailType.WrongData;
                return res;
            }
            
            var bookNotFound = false;
            foreach(var item in orderItems)
            {
                if(await _bookRepository.GetBook(item) == null)
                {
                    bookNotFound = true;
                    break;
                }
            }

            if (bookNotFound)
                res.Errors.Add("itemsNotFound", "Κάποια από τα βιβλία δεν υπάρχουν.");

            foreach(var item in orderItems)
            {
                if (await _bookRepository.GetBookStock(item) == 0)
                    res.Errors.Add($"book{item}", "Το βιβλίο δεν είναι διαθέσιμο.");
            }

            if (res.Errors.Count() > 0)
            {
                res.Success = false;
                res.ValidationFailType = ValidationFailType.WrongData;
            }

            return res;
        }
    }
}

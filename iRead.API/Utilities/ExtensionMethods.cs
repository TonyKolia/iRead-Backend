using iRead.API.Models;

namespace iRead.API.Utilities
{
    public static class ExtensionMethods
    {
        public static MemberPersonalInfoResponse MapResponse(this MemberPersonalInfo info)
        {
            return new MemberPersonalInfoResponse
            {
                Name = info.Name,
                Surname = info.Surname,
                Birthdate = info.Birthdate,
                IdType = info.IdType.ToString(),
                IdNumber = info.IdNumber
            };
        }

        public static MemberContactInfoResponse MapResponse(this MemberContactInfo info)
        {
            return new MemberContactInfoResponse
            {
                Address = info.Address,
                City = info.City,
                PostalCode = info.PostalCode,
                Telephone = info.Telephone,
                Email = info.Email,
            };
        }
    }
}

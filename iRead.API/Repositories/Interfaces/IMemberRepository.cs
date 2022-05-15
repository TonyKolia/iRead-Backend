using iRead.API.Models;

namespace iRead.API.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        #region Personal Info

        Task<MemberPersonalInfo> GetMemberPersonalInfo(int id);
        Task<MemberPersonalInfo> CreateMemberPersonalInfo(MemberPersonalInfo personalInfo);
        Task<MemberPersonalInfo> UpdateMemberPersonalInfo(MemberPersonalInfo personalInfo);
        Task<bool> IdNumberExists(string idNumber);

        #endregion

        #region Contact Info

        Task<MemberContactInfo> GetMemberContactInfo(int id);
        Task<MemberContactInfo> CreateMemberContactInfo(MemberContactInfo contactInfo);
        Task<MemberContactInfo> UpdateMemberContactInfo(MemberContactInfo contactInfo);

        #endregion

        Task<MemberResponse> GetMemberFullInfo(int id);

    }
}

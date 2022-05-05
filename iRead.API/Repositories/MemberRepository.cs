using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly iReadDBContext _db;

        public MemberRepository(iReadDBContext db)
        {
            this._db = db;
        }


        #region Personal Info

        public async Task<MemberPersonalInfo> GetMemberPersonalInfo(int id)
        {
            return await _db.MemberPersonalInfos.FirstOrDefaultAsync(x => x.UserId == id);
        }
        
        public async Task<MemberPersonalInfo> CreateMemberPersonalInfo(MemberPersonalInfo personalInfo)
        {
            _db.Entry(personalInfo).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return personalInfo;
        }

        public async Task<MemberPersonalInfo> UpdateMemberPersonalInfo(MemberPersonalInfo personalInfo)
        {
            var existingPersonalInfo = _db.MemberPersonalInfos.FirstOrDefault(x => x.UserId == personalInfo.UserId);
            if (existingPersonalInfo != null)
            {
                _db.Entry(personalInfo).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return personalInfo;
            }
            else
                return null;
        }

        #endregion

        #region Contact Info

        public async Task<MemberContactInfo> GetMemberContactInfo(int id)
        {
            return await _db.MemberContactInfos.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<MemberContactInfo> CreateMemberContactInfo(MemberContactInfo contactInfo)
        {
            _db.Entry(contactInfo).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return contactInfo;
        }

        public async Task<MemberContactInfo> UpdateMemberContactInfo(MemberContactInfo contactInfo)
        {
            var existingContactInfo = _db.MemberContactInfos.FirstOrDefault(x => x.UserId == contactInfo.UserId);
            if (existingContactInfo != null)
            {
                _db.Entry(contactInfo).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return contactInfo;
            }
            else
                return null;
        }

        #endregion

        public async Task<MemberFullInfo> GetMemberFullInfo(int id)
        {
            var userData = await _db.Users.Where(x => x.Id == id).Select(x => new { x.Username, x.RegisterDate }).FirstOrDefaultAsync();
            var memberPersonalInfo = await _db.MemberPersonalInfos.FirstOrDefaultAsync(x => x.UserId == id);
            var memberContactInfo = await _db.MemberContactInfos.FirstOrDefaultAsync(x => x.UserId == id);

            if (userData != null && (memberContactInfo != null || memberContactInfo != null))
                return new MemberFullInfo { Username = userData.Username, RegisterDate = userData.RegisterDate, ContactInfo = memberContactInfo, PersonalInfo = memberPersonalInfo };
            else
                return null;
        }
    }
}

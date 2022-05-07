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

        public async Task<MemberResponse> GetMemberFullInfo(int id)
        {
            var userData = await (from user in _db.Users
                            join pers in _db.MemberPersonalInfos on user.Id equals pers.UserId
                            join cont in _db.MemberContactInfos on user.Id equals cont.UserId
                            where user.Id == id
                            select new MemberResponse
                            {
                                Id = user.Id,
                                Username = user.Username,
                                RegisterDate = user.RegisterDate,
                                UserCategory = user.UserCategoryNavigation.Description ?? "",
                                PersonalInfo = new MemberPersonalInfoResponse
                                {
                                    Name = pers.Name,
                                    Surname = pers.Surname,
                                    Birthdate = pers.Birthdate.Value,
                                    IdType = pers.IdTypeNavigation.Description ?? "",
                                    IdNumber = pers.IdNumber
                                },
                                ContactInfo = new MemberContactInfoResponse
                                {
                                    Address = cont.Address,
                                    City = cont.City,
                                    PostalCode = cont.PostalCode,
                                    Telephone = cont.Telephone,
                                    Email = cont.Email
                                }
                            }).FirstOrDefaultAsync();

            return userData;
        }
    }
}

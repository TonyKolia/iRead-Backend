namespace iRead.API.Models
{
    public class MemberFullInfo
    {
        public MemberFullInfo()
        {
            PersonalInfo = new MemberPersonalInfo();
            ContactInfo = new MemberContactInfo();
        }

        public string Username { get; set; }
        public DateTime RegisterDate { get; set; }
        public MemberPersonalInfo PersonalInfo { get; set; }
        public MemberContactInfo ContactInfo { get; set; }
    }
}

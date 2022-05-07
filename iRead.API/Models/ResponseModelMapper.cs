namespace iRead.API.Models
{
    public class ResponseModelMapper
    {
        public PublisherResponse MapPublisher(Publisher publisher)
        {
            return new PublisherResponse
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Description = publisher.Description
            };
        }

        public IEnumerable<PublisherResponse> MapPublishers(IEnumerable<Publisher> publishers)
        {
            var mappedPublishers = new List<PublisherResponse>();

            foreach(var publisher in publishers)
            {
                mappedPublishers.Add(MapPublisher(publisher));
            }

            return mappedPublishers;
        }

        public MemberPersonalInfoResponse MapMemberPersonalInfo(MemberPersonalInfo info)
        {
            return new MemberPersonalInfoResponse
            {
                Name = info.Name,
                Surname = info.Surname,
                Birthdate = info.Birthdate.Value,
                IdType = info.IdType.ToString(),
                IdNumber = info.IdNumber
            };
        }

        public MemberContactInfoResponse MapMemberContactInfo(MemberContactInfo info)
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

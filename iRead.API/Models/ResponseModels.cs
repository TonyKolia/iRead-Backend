namespace iRead.API.Models
{
    public class Response<T>
    {
        public IEnumerable<T> Results { get; set; }

        public Response(IEnumerable<T> results = null)
        {
            Results = results ?? Array.Empty<T>();
        }
    }

    public class OrderResponse
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public IEnumerable<BookResponse> Books { get; set; } = new List<BookResponse>();
    }

    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PageCount { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal Rating { get; set; } = 0;
        public int TotalRatings { get; set; }
        public IEnumerable<AuthorResponse> Authors { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; }
        public IEnumerable<RatingResponse> Ratings { get; set; }
    }

    public class AuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
    }

    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class MemberResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime RegisterDate { get; set; }
        public string UserCategory { get; set; }
        public MemberPersonalInfoResponse PersonalInfo { get; set; }
        public MemberContactInfoResponse ContactInfo { get; set; }
    }

    public class MemberPersonalInfoResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
    }

    public class MemberContactInfoResponse
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

    }

    public class RatingResponse
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class PublisherResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GenderResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class IdentificationMethodResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    

}

namespace iRead.API.Models.Recommendation
{
    public class RelatedBookRecommendations
    {
        public IEnumerable<BookResponse> UserRecommendations { get; set; }
        public IEnumerable<BookResponse> OtherUsersRecommendations { get; set; }
        public IEnumerable<BookResponse> SimilarRecommendations { get; set; }

        public RelatedBookRecommendations(IEnumerable<BookResponse> UserRecommendations, IEnumerable<BookResponse> OtherUsersRecommendations, IEnumerable<BookResponse> SimilarRecommendations)
        {
            this.UserRecommendations = UserRecommendations;
            this.OtherUsersRecommendations = OtherUsersRecommendations;
            this.SimilarRecommendations = SimilarRecommendations;
        }

        public RelatedBookRecommendations()
        {

        }
    }


}

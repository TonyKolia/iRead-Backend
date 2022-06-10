namespace iRead.API.Models.Recommendation
{
    public class RecommendedBook
    {
        public int BookId { get; set; }
        public float PredictedRating { get; set; }

        public RecommendedBook(int bookId, float predictedRating)
        {
            this.BookId = bookId;
            this.PredictedRating = predictedRating;
        }
    }
}

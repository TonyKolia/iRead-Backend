using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRead.RecommendationSystem.Models
{
    public class TrainingInput
    {
        [KeyType(9999)]
        public uint UserId { get; set; }
        [KeyType(9999)]
        public uint BookId { get; set; }
        public float Rating { get; set; }

        public TrainingInput(uint userId, uint bookId, float rating)
        {
            this.UserId = userId;
            this.BookId = bookId;
            this.Rating = rating;
        }

        public TrainingInput() { }
    }
}

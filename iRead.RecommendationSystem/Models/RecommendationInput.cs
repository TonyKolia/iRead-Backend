using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRead.RecommendationSystem.Models
{
    public class RecommendationInput
    {
        [KeyType(9999)]
        public uint UserId { get; set; }
        [KeyType(9999)]
        public uint BookId { get; set; }

        public RecommendationInput(uint UserId, uint BookId)
        {
            this.UserId = UserId;
            this.BookId = BookId;
        }
    }
}

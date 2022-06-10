using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRead.DBModels.CustomModels
{
    public class RecommenderTrainingData
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Rating { get; set; }

        public RecommenderTrainingData() { }
    }
}

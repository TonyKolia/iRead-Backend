using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRead.RecommendationSystem.Models
{
    public class RecommendationOutput
    {
        [ColumnName("Score")]
        public float Rating { get; set; }
    }
}

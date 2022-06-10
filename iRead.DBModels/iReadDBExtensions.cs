using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using iRead.DBModels.CustomModels;

namespace iRead.DBModels.Models
{
    public partial class iReadDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build().GetConnectionString("iReadDBConnection"));
            }
        }

        public virtual DbSet<RecommenderTrainingData> TrainingInputs { get; set; } = null!;

        public IQueryable<RecommenderTrainingData> GetRecommenderTrainingData()
        {
            return this.TrainingInputs.FromSqlRaw("execute GetDataForModelTraining");
        }
    }

}

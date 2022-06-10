using iRead.RecommendationSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace iRead.RecommendationSystem
{
    public class Recommender
    {
        private MLContext _context  { get; set; }
        private IDataView? _trainingData { get; set; }
        private ITransformer _model { get; set; }
        private string _modelFilePath { get; set; }

        public Recommender(IEnumerable<TrainingInput> trainingData)
        {
            _context = new MLContext();
            _trainingData = _context.Data.LoadFromEnumerable(trainingData);
            _modelFilePath = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build().GetSection("RecommendationModelPath").Value;
        }

        public Recommender()
        {
            _context = new MLContext();
            _modelFilePath = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build().GetSection("RecommendationModelPath").Value;
        }

        public void SetTrainingData(IEnumerable<TrainingInput> trainingData)
        {
            _trainingData = _context.Data.LoadFromEnumerable(trainingData);
        }

        public ITransformer GetTrainedModel()
        {
            return _context.Model.Load(_modelFilePath, out var _modelSchema);
        }

        public void TrainModel()
        {
            var splitData = _context.Data.TrainTestSplit(_trainingData, testFraction: 0.2);
            var trainer = _context.Recommendation().Trainers.MatrixFactorization(SetupTrainer());
            _model = trainer.Fit(splitData.TrainSet);
            var evaluationPredictions = _model.Transform(splitData.TestSet);
            var metrics = _context.Regression.Evaluate(evaluationPredictions, labelColumnName: nameof(TrainingInput.Rating));
            System.Diagnostics.Debug.WriteLine($"RSquared: {metrics.RSquared:F2} - {metrics.RootMeanSquaredError}");
            _context.Model.Save(_model, _trainingData.Schema, _modelFilePath);
        }

        private MatrixFactorizationTrainer.Options SetupTrainer()
        {
            var options = new MatrixFactorizationTrainer.Options();
            options.MatrixColumnIndexColumnName = nameof(TrainingInput.BookId);
            options.MatrixRowIndexColumnName = nameof(TrainingInput.UserId);
            options.LabelColumnName = nameof(TrainingInput.Rating);
            options.NumberOfIterations = 30;
            return options;
        }

    }
}
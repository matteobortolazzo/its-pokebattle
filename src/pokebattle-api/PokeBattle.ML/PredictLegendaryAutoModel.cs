using Microsoft.ML;
using Microsoft.ML.AutoML;

namespace PokeBattle.ML;

public class PredictLegendaryAutoModel(string modelPath, string sourcePath)
{
    public LegendaryPrediction PredictLegendary(Pokemon pokemon)
    {
        ITransformer trainedModel;

        var mlContext = new MLContext(seed: 0);

        if (File.Exists(modelPath))
        {
            trainedModel = mlContext.Model.Load(modelPath, out var schema);
        }
        else
        {
            /*
            var columnInference =
                ctx.Auto().InferColumns(sourcePath, labelColumnName: "is_legendary", groupColumns: false);
            columnInference.ColumnInformation.NumericColumnNames.Remove("pokedex_number");
            columnInference.ColumnInformation.IgnoredColumnNames.Add("pokedex_name");
            columnInference.ColumnInformation.NumericColumnNames.Remove("category");
            columnInference.ColumnInformation.CategoricalColumnNames.Add("category");

            var loader = ctx.Data.CreateTextLoader(columnInference.TextLoaderOptions);
            var data = loader.Load(sourcePath);
            */
            var dataView = mlContext.Data.LoadFromTextFile<Pokemon>(
                sourcePath, hasHeader: true, separatorChar: ',');
            // Split data 80/20 for training and testing
            var trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2, seed: 0);

            SweepablePipeline pipeline =
                // ctx.Auto().Featurizer(data, columnInformation: columnInference.ColumnInformation)
                ProcessData(mlContext)
                    .Append(mlContext.Auto()
                        .BinaryClassification(labelColumnName: "Label"));
            AutoMLExperiment experiment = mlContext.Auto().CreateExperiment();
            experiment
                .SetPipeline(pipeline)
                .SetBinaryClassificationMetric(BinaryClassificationMetric.Accuracy,
                    labelColumn: "Label")
                .SetTrainingTimeInSeconds(60)
                .SetDataset(trainTestSplit);
            // Log experiment trials
            mlContext.Log += (_, e) =>
            {
                if (e.Source.Equals("AutoMLExperiment"))
                {
                    Console.WriteLine(e.RawMessage);
                }
            };
            TrialResult experimentResults = experiment.RunAsync().GetAwaiter().GetResult();
            trainedModel = experimentResults.Model;

            var predictions = trainedModel.Transform(trainTestSplit.TestSet);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions,
                labelColumnName: "Label");

            Console.WriteLine($"\nModel Evaluation Metrics:");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");

            mlContext.Model.Save(trainedModel, dataView.Schema, modelPath);
            Console.WriteLine($"\nModel saved to: {modelPath}");
        }

        var predEngine = mlContext.Model.CreatePredictionEngine<Pokemon, LegendaryPrediction>(trainedModel);
        var prediction = predEngine.Predict(pokemon);
        return prediction;
    }

    private static IEstimator<ITransformer> ProcessData(MLContext mlContext)
    {
        var pipeline =
            mlContext.Transforms.CopyColumns(inputColumnName: nameof(Pokemon.IsLegendary), outputColumnName: "Label")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    inputColumnName: nameof(Pokemon.Type1),
                    outputColumnName: "Type1Encoded"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    inputColumnName: nameof(Pokemon.Type2),
                    outputColumnName: "Type2Encoded"))
                .Append(mlContext.Transforms.Concatenate("Features",
                    nameof(Pokemon.Hp),
                    nameof(Pokemon.Attack),
                    nameof(Pokemon.SpecialAttack),
                    nameof(Pokemon.Defense),
                    nameof(Pokemon.SpecialDefence),
                    nameof(Pokemon.Speed),
                    "Type1Encoded",
                    "Type2Encoded"));
        return pipeline;
    }
}
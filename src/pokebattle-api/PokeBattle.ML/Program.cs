using PokeBattle.ML;

const string source = "/home/mborto/Repos/PokeBattle/data/pokemon.csv";

// Test with a sample Pokemon
var testPokemon = new Pokemon
{
    Hp = 100,
    Attack = 125,
    Defense = 90,
    SpecialAttack = 60,
    SpecialDefence = 70,
    Speed = 85
};
/* PREDICT TYPE * /
/*
const string modelPath = "/home/mborto/Repos/PokeBattle/data/predict-type.zip";
var predictTypeModel = new PredictTypeModel(modelPath, source);
var predictedType = predictTypeModel.PredictTypeModel(testPokemon)
*/

/* PREDICT TOTAL STATS */
/*
const string totalStatsModelPath = "/home/mborto/Repos/PokeBattle/data/predict-total-stats.zip";
var predictTotalStatsModel = new PredictTotalStatsModel(totalStatsModelPath, source);
var predictedTotal = predictTotalStatsModel.PredictTotalStat(testPokemon);
*/
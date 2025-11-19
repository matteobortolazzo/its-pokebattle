using PokeBattle.ML;

const string source = "/home/mborto/Repos/PokeBattle/data/pokemon.csv";

// Test with a sample Pokemon
var rillaboom = new Pokemon
{
    Hp = 100,
    Attack = 125,
    Defense = 90,
    SpecialAttack = 60,
    SpecialDefence = 70,
    Speed = 85,
    Type1 = "grass",
    // IsLegendary = false
};

var zacian = new Pokemon
{
    Hp = 92,
    Attack = 120,
    Defense = 115,
    SpecialAttack = 80,
    SpecialDefence = 115,
    Speed = 138,
    Type1 = "fairy",
    // IsLegendary = true
};

var eter = new Pokemon
{
    Hp = 140,
    Attack = 85,
    Defense = 95,
    SpecialAttack = 145,
    SpecialDefence = 95,
    Speed = 130,
    Type1 = "poison",
    Type2 = "dragon",
    // IsLegendary = true
};
var grookey = new Pokemon
{
    Hp = 50,
    Attack = 65,
    Defense = 50,
    SpecialAttack = 40,
    SpecialDefence = 40,
    Speed = 65,
    Type1 = "grass",
    // IsLegendary = false
};
var manafy = new Pokemon
{
    Hp = 100,
    Attack = 100,
    Defense = 100,
    SpecialAttack = 100,
    SpecialDefence = 100,
    Speed = 100,
    Type1 = "water",
    // IsLegendary = true
};

var dragapult = new Pokemon
{
    Hp = 88,
    Attack = 120,
    Defense = 75,
    SpecialAttack = 100,
    SpecialDefence = 75,
    Speed = 142,
    Type1 = "dragon",
    Type2 = "ghost",
    // IsLegendary = false
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

/* PREDICT LEGENDARY */
/*
const string legendaryModelPath = "/home/mborto/Repos/PokeBattle/data/predict-legendary.zip";
var predictLegendary = new PredictLegendaryModel(legendaryModelPath, source);
predictLegendary.PredictLegendary(grookey);
*/

/* PREDICT CATCH RATE */
/*
const string catchRatePath = "/home/mborto/Repos/PokeBattle/data/predict-catchrate.zip";
var predictTotalStatsModel = new PredictCatchRateModel(catchRatePath, source);
*/

/* PREDICT LEGENDARY AUTO */
const string legendaryModelPath = "/home/mborto/Repos/PokeBattle/data/predict-legendary-best.zip";
var predictLegendary = new PredictLegendaryAutoModel(legendaryModelPath, source);
predictLegendary.PredictLegendary(grookey);

// Test multiple Pokémon to see the pattern
var y1 = predictLegendary.PredictLegendary(grookey);
var y2 = predictLegendary.PredictLegendary(rillaboom);
var y3 = predictLegendary.PredictLegendary(zacian);
var y4 = predictLegendary.PredictLegendary(eter);
var y5 = predictLegendary.PredictLegendary(manafy);
var y6 = predictLegendary.PredictLegendary(dragapult);
Console.WriteLine("\n--- Testing Multiple Pokémon ---");
Console.WriteLine($"Grookey (not legendary): {y1.IsLegendary} {y1.Score}");
Console.WriteLine($"Rillaboom (not legendary): {y2.IsLegendary} {y2.Score}");
Console.WriteLine($"Zacian (legendary): {y3.IsLegendary} {y3.Score}");
Console.WriteLine($"Eternatus (legendary):{y4.IsLegendary} {y4.Score}");
Console.WriteLine($"Manafy (legendary):{y5.IsLegendary} {y5.Score}");
Console.WriteLine($"Dragapult (not legendary): {y6.IsLegendary} {y6.Score}");
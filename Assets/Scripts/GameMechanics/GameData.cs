using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public BuildingData[] BuildingStats;
	public CombatUnitData[] CombatUnitStats;
	public string[] ResourceNames;
	
	void Start () {
		ResourceNames = new string[(int)ResourceType.Count];
		ResourceNames[(int)ResourceType.Population] = "Population";
		ResourceNames[(int)ResourceType.Happiness] = "Happiness";
		ResourceNames[(int)ResourceType.Energy] = "Energy";
		ResourceNames[(int)ResourceType.Food] = "Food";
		ResourceNames[(int)ResourceType.Iron] = "Iron";
		ResourceNames[(int)ResourceType.Silicon] = "Silicon";
		ResourceNames[(int)ResourceType.Money] = "Money";



		BuildingStats = new BuildingData[(int)BuildingType.Count];
		int t = (int)BuildingType.EmptySpace;


		// empty space
		BuildingStats[t] = new BuildingData();
		BuildingStats[t].Name = "Empty space";
		BuildingStats[t].Description = "You can build something here!";
		BuildingStats[t].LevelUpText = "You can't level up. If you see this text, it's a bug.";
		BuildingStats[t].WorkerName = "error";
		BuildingStats[t].WorkerFunction = "error";

		// the following values for empty space have no meaning
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Population] = 5f; // the population that is put at work
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Energy] = 5f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Food] = 5f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Money] = 10f;
		
		BuildingStats[t].BaseProductivity[(int)ResourceType.Population] = 1f; // in this case, this is the max amount of troops stationed
		BuildingStats[t].BaseProductivity[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Energy] = -1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Food] = -1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Money] = -1f;
		
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Population] = 2f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Happiness] = 1f; // pacifists :D
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Energy] = 5f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Food] = 5f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Silicon] = 2f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Money] = 10f;
		
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Population] = 5f; // in this case, this is the max amount of troops stationed
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Happiness] = 0.05f; // people feel stronger when they have a strong army
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Energy] = -1f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Food] = -1f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Money] = 0f;
		
		BuildingStats[t].MultBuildCost[(int)ResourceType.Population] = 1.2f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Happiness] = 0.75f; // pacifists :D
		BuildingStats[t].MultBuildCost[(int)ResourceType.Energy] = 1.5f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Food] = 0.9f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Silicon] = 1.3f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Money] = 1.5f;
		
		BuildingStats[t].MultProductivity[(int)ResourceType.Population] = 1.3f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Happiness] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Energy] = 1.3f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Food] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Silicon] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Money] = 1f;
		
		BuildingStats[t].BaseBuildTime = 10f;
		BuildingStats[t].PerLevelBuildTime = 50f;
		BuildingStats[t].MultBuildTime = 1.5f;



		// barracks
		t = (int)BuildingType.Barracks;
		BuildingStats[t] = new BuildingData();
		BuildingStats[t].Name = "Barracks";
		BuildingStats[t].Description = "This is where your troops are stationed.";
		BuildingStats[t].LevelUpText = "Levelling up will make it possible for more\n" +
																"troops to be stationed.";
		BuildingStats[t].WorkerName = "recruit";
		BuildingStats[t].WorkerFunction = "stationed";

		BuildingStats[t].BaseBuildCost[(int)ResourceType.Population] = 5f; // the population that is put at work
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Energy] = 5f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Food] = 5f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Money] = 10f;

		BuildingStats[t].BaseProductivity[(int)ResourceType.Population] = 1f; // in this case, this is the max amount of troops stationed
		BuildingStats[t].BaseProductivity[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Energy] = -1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Food] = -1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Money] = -1f;

		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Population] = 2f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Happiness] = 1f; // pacifists :D
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Energy] = 5f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Food] = 5f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Silicon] = 2f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Money] = 10f;

		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Population] = 5f; // in this case, this is the max amount of troops stationed
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Happiness] = 0.05f; // people feel stronger when they have a strong army
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Energy] = -1f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Food] = -1f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Money] = 0f;

		BuildingStats[t].MultBuildCost[(int)ResourceType.Population] = 1.2f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Happiness] = 0.75f; // pacifists :D
		BuildingStats[t].MultBuildCost[(int)ResourceType.Energy] = 1.5f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Food] = 0.9f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Silicon] = 1.3f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Money] = 1.5f;

		BuildingStats[t].MultProductivity[(int)ResourceType.Population] = 1.3f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Happiness] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Energy] = 1.3f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Food] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Silicon] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Money] = 1f;

		BuildingStats[t].BaseBuildTime = 10f;
		BuildingStats[t].PerLevelBuildTime = 50f;
		BuildingStats[t].MultBuildTime = 1.5f;



		// iron mines
		t = (int)BuildingType.IronMine;
		BuildingStats[t] = new BuildingData();
		BuildingStats[t].Name = "Iron Mine";
		BuildingStats[t].Description = "This is where your iron is mined.";
		BuildingStats[t].LevelUpText = "Levelling up will make it possible for more\n" +
			"iron to be mined.";
		BuildingStats[t].WorkerName = "miner";
		BuildingStats[t].WorkerFunction = "employed";
		
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Population] = 8f; // the population that is put at work to build this structure
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Energy] = 10f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Food] = 8f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Money] = 20f;
		
		BuildingStats[t].BaseProductivity[(int)ResourceType.Population] = 5f; // in this case, this is the max amount of miners
		BuildingStats[t].BaseProductivity[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Energy] = -2f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Food] = -1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Money] = -0.5f;
		
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Population] = 4f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Energy] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Food] = 5f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Silicon] = 5f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Money] = 20f;
		
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Population] = 5f; // in this case, this is the max amount of miners
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Energy] = -0.1f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Food] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Silicon] = 1f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Money] = -0.1f;
		
		BuildingStats[t].MultBuildCost[(int)ResourceType.Population] = 1.5f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Happiness] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Energy] = 1.5f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Food] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Silicon] = 1.3f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Money] = 1.5f;
		
		BuildingStats[t].MultProductivity[(int)ResourceType.Population] = 2f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Happiness] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Energy] = 1.5f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Food] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Silicon] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Money] = 1f;
		
		BuildingStats[t].BaseBuildTime = 10f;
		BuildingStats[t].PerLevelBuildTime = 10f;
		BuildingStats[t].MultBuildTime = 3f;



		// skyscrapers
		t = (int)BuildingType.Skyscraper;
		BuildingStats[t] = new BuildingData();
		BuildingStats[t].Name = "Skyscraper";
		BuildingStats[t].Description = "This is where your businessmen meet and your\n" +
			"wealthier citizens work.";
		BuildingStats[t].LevelUpText = "Levelling up will make it possible for the building\n" +
			"to hold more people.";
		BuildingStats[t].WorkerName = "citizen";
		BuildingStats[t].WorkerFunction = "employed";
		
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Population] = 10f; // the population that is put at work to build this structure
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Energy] = 20f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Food] = 10f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Iron] = 20f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseBuildCost[(int)ResourceType.Money] = 50f;
		
		BuildingStats[t].BaseProductivity[(int)ResourceType.Population] = 10f; // in this case, this is the max amount of citizens at work
		BuildingStats[t].BaseProductivity[(int)ResourceType.Happiness] = 0.05f; // wealthier citizens = happier citizens
		BuildingStats[t].BaseProductivity[(int)ResourceType.Energy] = -2f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Food] = -1f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].BaseProductivity[(int)ResourceType.Money] = 1f;
		
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Population] = 1f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Energy] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Food] = 1f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Iron] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Silicon] = 10f;
		BuildingStats[t].PerLevelBuildCost[(int)ResourceType.Money] = 100f;
		
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Population] = 5f; // in this case, this is the max amount of citizens at work
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Happiness] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Energy] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Food] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Iron] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Silicon] = 0f;
		BuildingStats[t].PerLevelProductivity[(int)ResourceType.Money] = 1f;
		
		BuildingStats[t].MultBuildCost[(int)ResourceType.Population] = 1.5f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Happiness] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Energy] = 1.5f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Food] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Silicon] = 1.3f;
		BuildingStats[t].MultBuildCost[(int)ResourceType.Money] = 2f;
		
		BuildingStats[t].MultProductivity[(int)ResourceType.Population] = 1.2f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Happiness] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Energy] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Food] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Iron] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Silicon] = 1f;
		BuildingStats[t].MultProductivity[(int)ResourceType.Money] = 1.2f;
		
		BuildingStats[t].BaseBuildTime = 30f;
		BuildingStats[t].PerLevelBuildTime = 30f;
		BuildingStats[t].MultBuildTime = 2f;




		// combat units
		CombatUnitStats = new CombatUnitData[(int)CombatUnitType.Count];


		// soldiers
		t = (int)CombatUnitType.Soldier;
		CombatUnitStats[t] = new CombatUnitData();
		CombatUnitStats[t].Name = "soldier";
		CombatUnitStats[t].Description = "A soldier is cheap to train, but\n" +
										 "only has a little health and damage.";

		CombatUnitStats[t].TrainingCost[(int)ResourceType.Population] = 1f;
		CombatUnitStats[t].TrainingCost[(int)ResourceType.Happiness] = 0.1f;
		CombatUnitStats[t].TrainingCost[(int)ResourceType.Energy] = 1f;
		CombatUnitStats[t].TrainingCost[(int)ResourceType.Food] = 1f;
		CombatUnitStats[t].TrainingCost[(int)ResourceType.Iron] = 1f;
		CombatUnitStats[t].TrainingCost[(int)ResourceType.Silicon] = 1f;
		CombatUnitStats[t].TrainingCost[(int)ResourceType.Money] = 2f;

		CombatUnitStats[t].Health = 1f;
		CombatUnitStats[t].AttackDamage = 0.5f;
		CombatUnitStats[t].AttackTime = 1f;
		CombatUnitStats[t].MovementSpeed = 2f;
		CombatUnitStats[t].Weight = 1f;
		CombatUnitStats[t].MaxCarriedWeight = 1f;
		CombatUnitStats[t].CanCarryUnits = false;
	}

	public float GetBuildCost (BuildingType buildType, ResourceType resType, int currlevel) {
		float f = BuildingStats[(int)buildType].BaseBuildCost[(int)resType];
		for (int i = 0; i < currlevel; i++) {
			f += BuildingStats[(int)buildType].PerLevelBuildCost[(int)resType];
			f *= BuildingStats[(int)buildType].MultBuildCost[(int)resType];
		}
		return f;
	}

	public float GetProductivity (BuildingType buildType, ResourceType resType, int currlevel) {
		float f = BuildingStats[(int)buildType].BaseProductivity[(int)resType];
		for (int i = 1; i < currlevel; i++) {
			f += BuildingStats[(int)buildType].PerLevelProductivity[(int)resType];
			f *= BuildingStats[(int)buildType].MultProductivity[(int)resType];
		}
		return f;
	}

	public float GetBuildTime (BuildingType buildType, int currLevel) {
		float f = BuildingStats[(int)buildType].BaseBuildTime;
		for (int i = 0; i < currLevel; i++) {
			f += BuildingStats[(int)buildType].PerLevelBuildTime;
			f *= BuildingStats[(int)buildType].MultBuildTime;
		}
		return f;
	}
}

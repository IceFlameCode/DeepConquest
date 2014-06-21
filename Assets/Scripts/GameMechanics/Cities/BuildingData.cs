using UnityEngine;
using System.Collections;

// this is used to hold building type info for different levels
public class BuildingData {

	public string Name = "Building";
	public string Description = "This is an uninitialized building.";
	public string LevelUpText = "Levelling up won't change anything.";
	public string WorkerName = "worker";
	public string WorkerFunction = "employed";

	
	// Base - at level 0

	// build cost
	// the population is not consumed, but works on the building
	public float[] BaseBuildCost;

	// productivity (also used for costs)
	// population = the amount of employable workers, NOT children born :)
	// all values for resources are values per worker employed per hour
	public float[] BaseProductivity;



	// increase in stats per level
	// also affected by mult

	// build cost
	public float[] PerLevelBuildCost;

	// productivity
	public float[] PerLevelProductivity;



	// stat multiply per level (to achieve more control and less predictability)
	// see evaluation functions to learn how it works

	// build cost
	public float[] MultBuildCost;
	
	// productivity
	public float[] MultProductivity;



	// build time
	public float BaseBuildTime;
	public float PerLevelBuildTime;
	public float MultBuildTime;



	public BuildingData() {
		BaseBuildCost = new float[(int)ResourceType.Count];
		BaseProductivity = new float[(int)ResourceType.Count];
		PerLevelBuildCost = new float[(int)ResourceType.Count];
		PerLevelProductivity = new float[(int)ResourceType.Count];
		MultBuildCost = new float[(int)ResourceType.Count];
		MultProductivity = new float[(int)ResourceType.Count];
	}
	
}

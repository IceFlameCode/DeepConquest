using UnityEngine;
using System.Collections;

public enum HudMode {
	Login, // also handles registering
	Settings,
	CityView,
	WorldView,
	PlanetView,

	Count
}

public enum BuildingType {
	EmptySpace,
	IronMine,
	Barracks,
	Skyscraper,

	Count
}

public enum ResourceType {
	Population,
	Happiness,
	Energy,
	Food,
	Iron,
	Silicon,
	Money,

	Count
}

public enum CombatUnitType {
	Soldier,
	Mech,

	Count
}
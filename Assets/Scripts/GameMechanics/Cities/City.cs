using UnityEngine;
using System.Collections;

public class City {

	public Building[,] Buildings;
	public string Name;
	public Building SelectedBuilding;
	public bool BuildingSelected = false;

	public GameData GameStats;

	public float[] ResourcesInside; // all are floats for stat-over-time purposes

	public GameObject BarracksObject;
	public GameObject EmptySpaceObject;
	public GameObject IronMineObject;
	public GameObject SkyscraperObject;

	public City(GameData gameData) {
		Buildings = new Building[S.CitySize, S.CitySize];
		for (int i = 0; i < S.CitySize; i++)
		for (int j = 0; j < S.CitySize; j++) {
			Buildings[i, j] = new Building();
			Buildings[i, j].x = i;
			Buildings[i, j].y = j;
		}

		ResourcesInside = new float[(int)ResourceType.Count];
		ResourcesInside[(int)ResourceType.Population] = 10f;
		ResourcesInside[(int)ResourceType.Happiness] = 10f;
		ResourcesInside[(int)ResourceType.Energy] = 100f;
		ResourcesInside[(int)ResourceType.Food] = 100f;
		ResourcesInside[(int)ResourceType.Iron] = 100f;
		ResourcesInside[(int)ResourceType.Silicon] = 100f;
		ResourcesInside[(int)ResourceType.Money] = 100f;

		Name = "City";
		GameStats = gameData;
	}

	public string Encode() {
		//MonoBehaviour.print("Encoding city " + Name);
		string s = "";
		for (int i = 0; i < S.CitySize; i++)
		for (int j = 0; j < S.CitySize; j++) {
			//MonoBehaviour.print("Encoding building at " + i.ToString() + ", " + j.ToString());
			s += Buildings[i, j].Encode();
		}
		return s;
	}

	public void Decode(string s) {
		for (int i = 0; i < s.Length; i += 9) {
			Buildings[Mathf.FloorToInt(i / 9f / S.CitySize), (i / 9) % S.CitySize].Decode(s.Substring(i, 9));
		}
	}

	public void Build() {
		EmptySpaceObject = GameObject.Find("EmptySpace");
		BarracksObject = GameObject.Find("Barracks");
		IronMineObject = GameObject.Find("IronMine");
		SkyscraperObject = GameObject.Find("Skyscraper");
		for (int i = 0; i < S.CitySize; i++)
		for (int j = 0; j < S.CitySize; j++) {
			GameObject nb = (GameObject)GameObject.Instantiate(EmptySpaceObject);
			switch (Buildings[i, j].type) {
			case BuildingType.EmptySpace:
				GameObject.Destroy(nb);
				nb = (GameObject)GameObject.Instantiate(EmptySpaceObject);
				break;
			case BuildingType.Barracks:
				GameObject.Destroy(nb);
				nb = (GameObject)GameObject.Instantiate(BarracksObject);
				break;
			case BuildingType.IronMine:
				GameObject.Destroy(nb);
				nb = (GameObject)GameObject.Instantiate(IronMineObject);
				break;
			case BuildingType.Skyscraper:
				GameObject.Destroy(nb);
				nb = (GameObject)GameObject.Instantiate(SkyscraperObject);
				break;
			default:
				MonoBehaviour.print ("Unhandled building type in build = " + Buildings[i, j].type.ToString());
				break;
			}
			nb.transform.position = new Vector3(i, 0f, j);
			BuildingObject bo = nb.GetComponent<BuildingObject>();
			bo.x = i;
			bo.y = j;
			bo.CurrCity = this;
			Buildings[i, j].BuiltObject = nb;
		}
		GameObject.Destroy(EmptySpaceObject);
		GameObject.Destroy(BarracksObject);
		GameObject.Destroy(IronMineObject);
		GameObject.Destroy(SkyscraperObject);
	}

	public void UpdateBuildings() {
		for (int i = 0; i < S.CitySize; i++)
		for (int j = 0; j < S.CitySize; j++) {
			if (Time.time >= Buildings[i, j].BuildingFinishMoment && Buildings[i, j].isBeingBuilt) {
				ResourcesInside[(int)ResourceType.Population] += Buildings[i, j].WorkersAtWork;
				Buildings[i, j].WorkersAtWork = 0;
				Buildings[i, j].isBeingBuilt = false;
			}
		}
	}

	// returns gain in money, since that is shared between cities
	public float UpdateResources() {
		float moneyGain = 0f;
		for (int i = 0; i < S.CitySize; i++)
		for (int j = 0; j < S.CitySize; j++)
		for (int k = 0; k < (int)ResourceType.Count; k++) {
			switch ((ResourceType)k) {
				case ResourceType.Population:
					break;
				case ResourceType.Money:
					if (!Buildings[i, j].isBeingBuilt)
						moneyGain += GameStats.GetProductivity(Buildings[i, j].type, ResourceType.Money, Buildings[i, j].Level) * Buildings[i, j].WorkersAtWork * S.ResourceUpdateTime / 3600f;
					break;
				default:
					if (!Buildings[i, j].isBeingBuilt)
						ResourcesInside[k] += GameStats.GetProductivity(Buildings[i, j].type, (ResourceType)k, Buildings[i, j].Level) * Buildings[i, j].WorkersAtWork * S.ResourceUpdateTime / 3600f;
					break;
			}
		}
		return moneyGain;
	}
}

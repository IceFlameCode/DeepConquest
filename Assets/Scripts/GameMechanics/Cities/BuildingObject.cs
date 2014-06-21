using UnityEngine;
using System.Collections;

public class BuildingObject : MonoBehaviour {

	public City CurrCity;
	public int x;
	public int y;

	void OnMouseOver () {
		CurrCity.SelectedBuilding = CurrCity.Buildings[x, y];
		CurrCity.BuildingSelected = true;
	}

	void OnMouseExit () {
		CurrCity.BuildingSelected = false;
	}
}
